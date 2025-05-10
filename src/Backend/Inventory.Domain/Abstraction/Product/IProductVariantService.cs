using Inventory.Data.Models.Product;
using RedBook.Core.Models;
using RedBook.Core.UnitOfWork;
using RedBook.Core.AutoMapper;

namespace Inventory.Domain.Abstraction.Product
{
    public interface IProductVariantService
    {
        public Task<IEnumerable<ProductVariantModel>> GetVariantListOfProduct(int productId);
        public Task<IEnumerable<ProductVariantModel>> SaveNewVariantsAsync(IRepositoryFactory factory, IEnumerable<ProductVariantModel> productVariants);
        public Task<IEnumerable<ProductVariantModel>> SaveNewVariantsAsync(int ProductId, IEnumerable<ProductVariantModel> productVariants);
        public Task UpdateAsync(int VariantId, Dictionary<string, object> productVariant);
        public Task DeleteAsync(int productVariantId);
    }
}
