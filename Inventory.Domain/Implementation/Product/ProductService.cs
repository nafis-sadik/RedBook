using Inventory.Data.Models.Product;
using Inventory.Domain.Abstraction.Product;
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
            IUnitOfWorkManager unitOfWork
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork) { }

        public async Task<ProductModel> AddNewAsync(ProductModel productModel)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _productRepo = _repositoryFactory.GetRepository<Data.Entities.Product>();

                Data.Entities.Product product = Mapper.Map<Data.Entities.Product>(productModel);
                product.CreateBy = User.UserId;
                product.CreateDate = DateTime.UtcNow;
                product = await _productRepo.InsertAsync(product);

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

                var query = _productRepo.UnTrackableQuery();

                if (!string.IsNullOrWhiteSpace(pagedModel.SearchString))
                {
                    query = query.Where(x =>
                        x.OrganizationId == orgId
                        && x.ProductName.ToLower().Trim().Contains(pagedModel.SearchString.ToLower().Trim())
                    );
                }
                else
                {
                    query = query.Where(x => x.OrganizationId == orgId);
                }

                pagedModel.SourceData = await query
                    .Skip(pagedModel.Skip)
                    .Take(pagedModel.PageLength)
                    .Select(x => new ProductModel
                    {
                        ProductId = x.ProductId,
                        ProductName = x.ProductName,
                        SubcategoryId = x.CategoryId,
                        SubcategoryName = x.Category.CatagoryName,
                        CategoryId = (int)x.Category.ParentCategoryId,
                        CategoryName = x.Category.ParentCategory.CatagoryName.ToString(),
                        OrganizationId = x.OrganizationId,
                        QuantityTypeId = x.QuantityAttributeId
                    })
                    .ToArrayAsync();

                pagedModel.TotalItems = await query.CountAsync();

                return pagedModel;
            }
        }

        public async Task<ProductModel> UpdateAsync(ProductModel productModel)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _productRepo = _repositoryFactory.GetRepository<Data.Entities.Product>();

                Data.Entities.Product? product = await _productRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.ProductId == productModel.ProductId);

                if (product == null) throw new ArgumentException("Resource not found");

                product.ProductName = productModel.ProductName;
                product.CategoryId = productModel.SubcategoryId;
                product.UpdateBy = User.UserId;
                product.UpdateDate = DateTime.UtcNow;

                _productRepo.Update(product);

                await _repositoryFactory.SaveChangesAsync();

                return Mapper.Map<ProductModel>(productModel);
            }
        }
    }
}
