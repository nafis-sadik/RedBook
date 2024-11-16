using Inventory.Data.Models.Purchase;

namespace Inventory.Domain.Abstraction.Purchase
{
    public interface IInvoiceService
    {
        public Task<PurchaseInvoiceModel> GetByIdAsync(int id);
        public Task<PagedInvoiceModel> GetPagedAsync(PagedInvoiceModel pagedModel);
        public Task<PurchaseInvoiceModel> AddNewAsync(PurchaseInvoiceModel model);
        public Task<PurchaseInvoiceModel> UpdateAsync(int id, Dictionary<string, object> updates);
        public Task DeleteAsync(int id);
    }
}
