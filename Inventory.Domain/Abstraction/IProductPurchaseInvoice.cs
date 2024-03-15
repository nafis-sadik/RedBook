using Inventory.Data.Models;
using RedBook.Core.Models;

namespace Inventory.Domain.Abstraction
{
    public interface IProductPurchaseInvoice
    {
        public Task AddNewInvoiceAsync(PurchaseModel purchaseModel);
        public Task<PagedPurchaseInvoiceModel> GetPagedInvoiceAsync(PagedPurchaseInvoiceModel purchaseModel);
        public Task<PurchaseModel> UpdateExistingInvoiceAsync(PurchaseModel purchaseModel);
        public Task RemoveInvoiceAsync(int id);
    }
}
