using Inventory.Data.Entities;
using Inventory.Data.Models.Product;
using Inventory.Domain.Abstraction.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Domain;
using RedBook.Core.Models;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RedBook.Core;


namespace Inventory.Domain.Implementation.Product
{
    public class ProductService : ServiceBase, IProductService
    {
        private IHttpContextAccessor _contextAccessor;
        private IClaimsPrincipalAccessor _claimsPrincipalAccessor;
        public ProductService(
            ILogger<ProductService> logger,
            IObjectMapper mapper,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IUnitOfWorkManager unitOfWork,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor) {
            _contextAccessor = httpContextAccessor;
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
        }

        public async Task<ProductModel> AddNewAsync(ProductModel productModel)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _productRepo = _repositoryFactory.GetRepository<Data.Entities.Product>();
                var productVariantRepo = _repositoryFactory.GetRepository<ProductVariant>();

                Data.Entities.Product product = Mapper.Map<Data.Entities.Product>(productModel);
                product.CreateBy = User.UserId;
                product.CreateDate = DateTime.UtcNow;
                product.ProductVariants.Clear();
                foreach(ProductVariantModel variant in productModel.ProductVariants)
                {
                    product.ProductVariants.Add(new ProductVariant
                    {
                        VariantName = variant.VariantName,
                        Attributes = variant.Attributes,
                        SKU = variant.SKU,
                        StockQuantity = 0,
                        CreateBy = User.UserId,
                        CreateDate = DateTime.UtcNow,
                        BarCode = Guid.NewGuid().ToString(),
                        IsActive = true
                    });
                }
                product = await _productRepo.InsertAsync(product);

                IEnumerable<ProductVariant> variantList = Mapper.Map<IEnumerable<ProductVariant>>(productModel.ProductVariants);
                foreach (ProductVariant entity in variantList)
                {
                    entity.CreateBy = User.UserId;
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateBy = null;
                    entity.UpdateDate = null;
                    entity.IsActive = true;
                }

                await productVariantRepo.BulkInsertAsync(variantList);

                await _repositoryFactory.SaveChangesAsync();

                return Mapper.Map<ProductModel>(product);
            }
        }

        public async Task DeleteProductAsync(int categoryId)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _productRepo = _repositoryFactory.GetRepository<Data.Entities.Product>();

                await _productRepo.DeleteAsync(categoryId);

                await _repositoryFactory.SaveChangesAsync();
            }
        }

        public async Task<PagedModel<ProductModel>> GetPagedAsync(PagedModel<ProductModel> pagedModel, int orgId)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _productRepo = _repositoryFactory.GetRepository<Data.Entities.Product>();
                var _purchaseVariantRepo = _repositoryFactory.GetRepository<ProductVariant>();
                var _purchaseDetailsRepo = _repositoryFactory.GetRepository<PurchaseInvoiceDetails>();

                var query = _productRepo.UnTrackableQuery().Where(x => x.OrganizationId == orgId);

                if (!string.IsNullOrWhiteSpace(pagedModel.SearchString))
                    query = query.Where(x => x.ProductName.ToLower().Trim().Contains(pagedModel.SearchString.ToLower().Trim()));

                pagedModel.SourceData = await query
                    .Skip(pagedModel.Skip)
                    .Take(pagedModel.PageLength)
                    .Select(product => new ProductModel
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        SubcategoryId = product.CategoryId,
                        SubcategoryName = product.Category.CatagoryName,
                        CategoryId = product.Category.ParentCategoryId == null? 0 : product.Category.ParentCategoryId.Value,
                        CategoryName = product.Category.ParentCategoryId == null ? "Empty Category Name" : product.Category.ParentCategory.CatagoryName.ToString(),
                        PurchasePrice = (float)_purchaseDetailsRepo.UnTrackableQuery()
                                        .Where(purchaseDetails => purchaseDetails.ProductVariantId == product.ProductId)
                                        .OrderByDescending(purchaseDetails => purchaseDetails.CreateDate)
                                        .Select(purchaseDetails => purchaseDetails.PurchasePrice)
                                        .FirstOrDefault(),
                        RetailPrice = (float)_purchaseDetailsRepo.UnTrackableQuery()
                                        .Where(purchaseDetails => purchaseDetails.ProductVariantId == product.ProductId)
                                        .OrderByDescending(purchaseDetails => purchaseDetails.CreateDate)
                                        .Select(purchaseDetails => purchaseDetails.RetailPrice)
                                        .FirstOrDefault(),
                        OrganizationId = product.OrganizationId,
                        BrandName = product.BrandAttribute.AttributeName,
                        BrandId = product.BrandId,
                        QuantityTypeId = product.QuantityAttributeId,
                        ProductVariants = _purchaseVariantRepo.UnTrackableQuery()
                                        .Where(variant => variant.IsActive && variant.ProductId == product.ProductId)
                                        .Select(variant => new ProductVariantModel
                                        {
                                            ProductId = product.ProductId,
                                            VariantId = variant.VariantId,
                                            VariantName = variant.VariantName,
                                            SKU = variant.SKU,
                                            BarCode = variant.BarCode,
                                            Attributes = variant.Attributes,
                                            StockQuantity = variant.StockQuantity
                                        }).ToList()
                    })
                    .ToArrayAsync();

                pagedModel.TotalItems = await query.CountAsync();

                return pagedModel;
            }
        }

        public async Task<IEnumerable<ProductModel>> GetListByOrgIdAsync(int orgId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _productRepo = factory.GetRepository<Data.Entities.Product>();

                return await _productRepo.UnTrackableQuery().Where(x => x.OrganizationId == orgId)
                    .Select(x => new ProductModel
                    {
                        ProductId= x.ProductId,
                        ProductName = x.ProductName,
                        ProductVariants = x.ProductVariants.Where(variant => variant.IsActive).Select(variant => new ProductVariantModel
                        {
                            VariantId = variant.VariantId,
                            VariantName = variant.VariantName,
                        }).ToList()
                    })
                    .ToListAsync();
            }
        }

        public async Task<ProductModel> UpdateAsync(int productId, Dictionary<string, object> productModel)
        {
            // Variant entity object generation
            string? productVariantsJson = productModel["productVariants"].ToString();
            if (string.IsNullOrWhiteSpace(productVariantsJson)) throw new ArgumentException("Product must have atleast 1 variant");
            object[]? productVariantArr = JsonConvert.DeserializeObject<object[]>(productVariantsJson);
            List<ProductVariantModel> productVariants = new List<ProductVariantModel>();
            if (productVariantArr == null) {
                Logger.LogError($"{DateTime.UtcNow} :: ProductService :: UpdateAsync :: Failed to generate productVariants from productModel.");
                throw new ArgumentException("Failed to generate variant model");
            } else {
                foreach(JObject variantData in productVariantArr)
                {
                    ProductVariantModel variant = new ProductVariantModel();
                    variant.VariantId = variantData.ContainsKey("variantId") ? int.Parse(variantData.GetValue("variantId")?.ToString()!) : 0;
                    variant.SKU = variantData.ContainsKey("sku") ? variantData.GetValue("sku")?.ToString()! : string.Empty;
                    variant.Attributes = variantData.ContainsKey("attributes") ? variantData.GetValue("attributes")?.ToString()! : string.Empty;
                    variant.VariantName = variantData.ContainsKey("variantName") ? variantData.GetValue("variantName")?.ToString()! : string.Empty;
                    variant.ProductId = productId;
                    variant.StockQuantity = decimal.Zero;
                    variant.BarCode = Guid.NewGuid().ToString();

                    productVariants.Add(variant);
                }
            }

            // Product view model generation
            productModel["UpdateBy"] = User.UserId;
            productModel["UpdateDate"] = DateTime.UtcNow;
            productModel["categoryId"] = productModel["subcategoryId"];

            // Database operation
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                // Update product table data
                var _productRepo = factory.GetRepository<Data.Entities.Product>();

                _productRepo.ColumnUpdate(productId, productModel);

                await factory.SaveChangesAsync();

                // Update variant table data
                using var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
                ILogger<ProductVariantService> _productVariantLogger = loggerFactory.CreateLogger<ProductVariantService>();

                ProductVariantService productVariantService = new ProductVariantService(
                    logger: _productVariantLogger,
                    mapper: Mapper,
                    claimsPrincipalAccessor: _claimsPrincipalAccessor,
                    unitOfWork: UnitOfWorkManager,
                    httpContextAccessor: _contextAccessor
                );
                productVariants = (await productVariantService.SaveNewVariantsAsync(factory, productVariants)).ToList();

                await factory.SaveChangesAsync();
            }

            ProductModel productViewModel = productModel.ToObject<ProductModel>();
            productViewModel.ProductVariants = productVariants;

            return productViewModel;
        }
    }
}
