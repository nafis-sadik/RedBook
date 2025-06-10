using Inventory.Data.Models.Purchase;

namespace Inventory.Domain.Abstraction.Sales
{
    public interface IInventoryService
    {
        public Task<IEnumerable<PurchaseInvoiceDetailsModel>> GetInventoryOfVariant (int productVariantId);
    }
}
