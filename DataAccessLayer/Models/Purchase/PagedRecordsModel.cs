using RedBook.Core.Models;

namespace Inventory.Data.Models.Purchase
{
    public class PagedRecordsModel : PagedModel<InvoiceDetailsModel>
    {
        public int InvoiceId { get; set; }
    }
}
