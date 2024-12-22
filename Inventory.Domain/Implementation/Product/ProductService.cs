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

namespace Inventory.Domain.Implementation.Product
{
    public class ProductService : ServiceBase, IProductService
    {
        public ProductService(
            ILogger<ProductService> logger,
            IObjectMapper mapper,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IUnitOfWorkManager unitOfWork,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor) { }

        public async Task<ProductModel> AddNewAsync(ProductModel productModel)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _productRepo = _repositoryFactory.GetRepository<Data.Entities.Product>();
                var productVariantRepo = _repositoryFactory.GetRepository<ProductVariant>();

                Data.Entities.Product product = Mapper.Map<Data.Entities.Product>(productModel);
                product.CreateBy = User.UserId;
                product.CreateDate = DateTime.UtcNow;
                product = await _productRepo.InsertAsync(product);
                await _repositoryFactory.SaveChangesAsync();

                IEnumerable<ProductVariant> entityList = Mapper.Map<IEnumerable<ProductVariant>>(productModel.ProductVariants);
                foreach (ProductVariant entity in entityList) { 
                    entity.CreateBy = User.UserId;
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateBy = null;
                    entity.UpdateDate = null;
                    entity.IsActive = true;
                }

                await productVariantRepo.BulkInsertAsync(entityList);

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
                var _purchaseDetailsRepo = _repositoryFactory.GetRepository<Data.Entities.PurchaseInvoiceDetails>();

                var query = _productRepo.UnTrackableQuery().Where(x => x.OrganizationId == orgId);

                if (!string.IsNullOrWhiteSpace(pagedModel.SearchString))
                    query = query.Where(x => x.ProductName.ToLower().Trim().Contains(pagedModel.SearchString.ToLower().Trim()));

                pagedModel.SourceData = await query
                    .Skip(pagedModel.Skip)
                    .Take(pagedModel.PageLength)
                    .Select(x => new ProductModel
                    {
                        ProductId = x.ProductId,
                        ProductName = x.ProductName,
                        SubcategoryId = x.CategoryId,
                        SubcategoryName = x.Category.CatagoryName,
                        CategoryId = x.Category.ParentCategoryId == null? 0 : x.Category.ParentCategoryId.Value,
                        CategoryName = x.Category.ParentCategoryId == null ? "Empty Category Name" : x.Category.ParentCategory.CatagoryName.ToString(),
                        PurchasePrice = (float)_purchaseDetailsRepo.UnTrackableQuery()
                                        .Where(purchaseDetails => purchaseDetails.ProductVariantId == x.ProductId)
                                        .OrderByDescending(purchaseDetails => purchaseDetails.CreateDate)
                                        .Select(purchaseDetails => purchaseDetails.PurchasePrice)
                                        .FirstOrDefault(),
                        RetailPrice = (float)_purchaseDetailsRepo.UnTrackableQuery()
                                        .Where(purchaseDetails => purchaseDetails.ProductVariantId == x.ProductId)
                                        .OrderByDescending(purchaseDetails => purchaseDetails.CreateDate)
                                        .Select(purchaseDetails => purchaseDetails.RetailPrice)
                                        .FirstOrDefault(),
                        OrganizationId = x.OrganizationId,
                        BrandName = x.BrandAttribute.AttributeName,
                        BrandId = x.BrandId,
                        QuantityTypeId = x.QuantityAttributeId,
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
                    })
                    .ToListAsync();
            }
        }

        public async Task<ProductModel> UpdateAsync(int productId, Dictionary<string, object> productModel)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _productRepo = factory.GetRepository<Data.Entities.Product>();

                productModel["UpdateBy"] = User.UserId;
                productModel["UpdateDate"] = DateTime.UtcNow;
                productModel["categoryId"] = productModel["subcategoryId"];

                _productRepo.ColumnUpdate(productId, productModel);

                await factory.SaveChangesAsync();

                return Mapper.Map<ProductModel>(productModel);
            }
        }
    }
}
