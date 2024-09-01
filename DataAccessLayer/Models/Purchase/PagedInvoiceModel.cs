using RedBook.Core.Models;

namespace Inventory.Data.Models.Purchase
{
    public class PagedInvoiceModel : PagedModel<InvoiceModel>
    {
        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now;
    }
}
