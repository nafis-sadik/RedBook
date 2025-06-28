using Inventory.Data.Models.Sales;
using RedBook.Core.Models;

namespace Inventory.Domain.Abstraction
{
    public interface ISalesService
    {
        public Task<SalesInvoiceModel> Save(SalesInvoiceModel invoice);
        public Task<PagedModel<SalesInvoiceModel>> GetPaged(PagedModel<SalesInvoiceModel> pagedInvoice);
        public Task<SalesInvoiceModel> GetById(int invoiceId);
    }
}
