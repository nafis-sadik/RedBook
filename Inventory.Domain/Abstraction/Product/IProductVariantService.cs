using Inventory.Data.Models.Product;

namespace Inventory.Domain.Abstraction.Product
{
    public interface IProductVariantService
    {
        public Task<IEnumerable<ProductVariantModel>> GetVariantListOfProduct(int productId);
        public Task<IEnumerable<ProductVariantModel>> SaveNewVariantsAsync(IEnumerable<ProductVariantModel> productVariants);
        public Task UpdateAsync(int VariantId, Dictionary<string, object> productVariant);
        public Task DeleteAsync(int productVariantId);
    }
}
