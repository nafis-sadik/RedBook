using Inventory.Data.Entities;
using Inventory.Data.Models;
using Inventory.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Domain;
using RedBook.Core.Models;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Inventory.Domain.Implementation
{
    public class ProductService : ServiceBase, IProductService
    {
        private IRepositoryBase<Product> _productRepo;

        public ProductService(
            ILogger<ProductService> logger,
            IObjectMapper mapper,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IUnitOfWorkManager unitOfWork
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork) { }

        public async Task<ProductModel> AddNewProductAsync(ProductModel productModel)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _productRepo = unitOfWork.GetRepository<Product>();

                Product product = await _productRepo.InsertAsync(new Product
                {
                    ProductName = productModel.ProductName,
                    CategoryId = productModel.SubcategoryId <= 0 ? productModel.CategoryId : productModel.SubcategoryId,
                    CreateDate = DateTime.UtcNow,
                    CreateBy = User.UserId,
                    OrganizationId = productModel.OrganizationId
                });

                await unitOfWork.SaveChangesAsync();

                return Mapper.Map<ProductModel>(product);
            }
        }

        public async Task DeleteProductAsync(int categoryId)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _productRepo = unitOfWork.GetRepository<Product>();

                await _productRepo.DeleteAsync(categoryId);

                await unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<PagedModel<ProductModel>> GetProductsUnderOrganizationAsync(PagedModel<ProductModel> pagedModel, int orgId)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _productRepo = unitOfWork.GetRepository<Product>();

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
                        CategoryId = x.CategoryId,
                        CategoryName = x.Category.CatagoryName,
                        OrganizationId = x.OrganizationId
                    })
                    .ToArrayAsync();

                return pagedModel;
            }
        }

        public async Task<ProductModel> UpdateProductAsync(ProductModel productModel)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _productRepo = unitOfWork.GetRepository<Product>();

                Product? product = await _productRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.ProductId == productModel.ProductId);

                if (product == null) throw new ArgumentException("Resource not found");

                product.ProductName = productModel.ProductName;
                product.CategoryId = productModel.SubcategoryId;
                product.UpdateBy = User.UserId;
                product.UpdateDate = DateTime.UtcNow;

                _productRepo.Update(product);

                await unitOfWork.SaveChangesAsync();

                return Mapper.Map<ProductModel>(productModel);
            }
        }
    }
}
