using RedBook.Core.Models;

namespace Inventory.Data.Models.Purchase
{
    public class PagedRecordsModel : PagedModel<RecordModel>
    {
        public int InvoiceId { get; set; }
    }
}
