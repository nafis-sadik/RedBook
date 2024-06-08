using Inventory.Data.Models.Purchase;
using RedBook.Core.Models;

namespace Inventory.Domain.Abstraction.Purchase
{
    public interface IPurchaseInvoiceService
    {
        public Task<InvoiceModel> GetByIdAsync(int id);
        public Task<PagedInvoiceModel> GetPagedAsync(PagedInvoiceModel pagedModel);
        public Task<InvoiceModel> AddNewAsync(InvoiceModel model);
        public Task<InvoiceModel> UpdateAsync(int id, Dictionary<string, object> updates);
        public Task DeleteAsync(int id);
    }
}
