using RedBook.Core.Models;

namespace Inventory.Data.Models.Purchase
{
    public class PagedInvoiceModel : PagedModel<PurchaseInvoiceModel>
    {

        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime EndDate { get; set; } = DateTime.UtcNow;
    }
}
