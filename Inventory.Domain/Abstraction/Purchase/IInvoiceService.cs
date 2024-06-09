using Inventory.Data.Models.Purchase;

namespace Inventory.Domain.Abstraction.Purchase
{
    public interface IInvoiceService
    {
        public Task<InvoiceModel> GetByIdAsync(int id);
        public Task<PagedInvoiceModel> GetPagedAsync(PagedInvoiceModel pagedModel);
        public Task<InvoiceModel> AddNewAsync(InvoiceModel model);
        public Task<InvoiceModel> UpdateAsync(int id, Dictionary<string, object> updates);
        public Task DeleteAsync(int id);
    }
}
