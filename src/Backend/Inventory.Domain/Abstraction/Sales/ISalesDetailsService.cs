using Inventory.Data.Models.Sales;

namespace Inventory.Domain.Abstraction.Sales
{
    public interface ISalesDetailsService
    {
        public Task<List<SalesInvoiceDetailsModel>> GetByInvoiceId(int invoiceId);
    }
}
