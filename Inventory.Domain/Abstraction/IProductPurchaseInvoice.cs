using Inventory.Data.Models;
using RedBook.Core.Models;

namespace Inventory.Domain.Abstraction
{
    public interface IProductPurchaseInvoice
    {
        public Task AddNewInvoiceAsync(PurchaseModel purchaseModel);
        public Task<PagedModel<PurchaseModel>> GetPagedInvoiceAsync(PagedModel<PurchaseModel> purchaseModel);
        public Task<PurchaseModel> UpdateExistingInvoiceAsync(PurchaseModel purchaseModel);
        public Task RemoveInvoiceAsync(int id);
    }
}
