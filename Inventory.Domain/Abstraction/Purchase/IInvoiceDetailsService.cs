using Inventory.Data.Models.Purchase;
using RedBook.Core.Models;

namespace Inventory.Domain.Abstraction.Purchase
{
    public interface IInvoiceDetailsService
    {
        public Task<List<InvoiceDetailsModel>> AddNewAsync(List<InvoiceDetailsModel> purchaseModel);
        public Task<PagedModel<InvoiceDetailsModel>> GetPagedAsync(PagedModel<InvoiceDetailsModel> purchaseModel);
        public Task<IEnumerable<InvoiceDetailsModel>> GetListAsync(int invoiceId);
        public Task<InvoiceDetailsModel> UpdateAsync(InvoiceDetailsModel purchaseModel);
        public Task DeleteAsync(int id);
    }
}
