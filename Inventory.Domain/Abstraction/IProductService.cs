using Inventory.Data.Models;
using RedBook.Core.Models;

namespace Inventory.Domain.Abstraction
{
    public interface IProductService
    {
        public Task<ProductModel> AddNewProductAsync(ProductModel productModel);
        public Task<PagedModel<ProductModel>> GetProductsUnderOrganizationAsync(PagedModel<ProductModel> pagedModel, int orgId);
        public Task DeleteProductAsync(int categoryId);
        public Task<ProductModel> UpdateProductAsync(ProductModel productModel);
    }
}
