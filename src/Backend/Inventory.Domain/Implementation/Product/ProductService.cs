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
                //var productVariantRepo = _repositoryFactory.GetRepository<ProductVariant>();

                bool productExists = await _productRepo.UnTrackableQuery()
                    .AnyAsync(product => 
                        product.ProductName.ToLower().Equals(productModel.ProductName.ToLower())
                        && product.OrganizationId == productModel.OrganizationId
                    );

                if (productExists)
                    throw new ArgumentException($"Product {productModel.ProductName} already exists for your organization.");

                Data.Entities.Product productEntity = Mapper.Map<Data.Entities.Product>(productModel);
                productEntity.CreateBy = User.UserId;
                productEntity.CreateDate = DateTime.UtcNow;
                productEntity.UpdateBy = null;
                productEntity.UpdateDate = null;

                foreach (ProductVariant variant in productEntity.ProductVariants)
                {
                    variant.CreateBy = User.UserId;
                    variant.CreateDate = DateTime.UtcNow;
                    variant.UpdateBy = null;
                    variant.UpdateDate = null;
                    variant.BarCode = Guid.NewGuid().ToString();
                    variant.IsActive = true;

                    variant.Product = productEntity;
                }

                IEnumerable<ProductVariant> variantList = productEntity.ProductVariants;
                productEntity = await _productRepo.InsertAsync(productEntity);

                //await productVariantRepo.BulkInsertAsync(variantList);

                await _repositoryFactory.SaveChangesAsync();

                return Mapper.Map<ProductModel>(productEntity);
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
                        OrganizationId = product.OrganizationId,
                        BrandName = product.BrandAttribute.AttributeName,
                        BrandId = product.BrandId,
                        QuantityTypeId = product.QuantityAttributeId,
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
                        ProductVariants = x.ProductVariants.Where(variant => variant.IsActive)
                        .Select(variant => new ProductVariantModel
                        {
                            VariantId = variant.VariantId,
                            VariantName = variant.VariantName,
                            ProductId = variant.ProductId,
                            ProductName = variant.Product.ProductName
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
