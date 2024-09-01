using RedBook.Core.Models;

namespace Inventory.Data.Models.Purchase
{
    public class PagedInvoiceDetailsModel : PagedModel<InvoiceDetailsModel>
    {
        public int InvoiceId { get; set; }
    }
}
