using Inventory.Data.Models.Product;
using RedBook.Core.Models;

namespace Inventory.Domain.Abstraction.Product
{
    public interface IProductService
    {
        public Task<PagedModel<ProductModel>> GetPagedAsync(PagedModel<ProductModel> pagedModel, int orgId);
        public Task<IEnumerable<ProductModel>> GetListByOrgIdAsync(int orgId);
        public Task<ProductModel> AddNewAsync(ProductModel productModel);
        public Task UpdateAsync(int productId, Dictionary<string, object> productModel);
        public Task DeleteProductAsync(int categoryId);
    }
}
