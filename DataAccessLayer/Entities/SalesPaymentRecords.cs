using RedBook.Core.EntityFramework;

namespace Inventory.Domain.Entities
{
    public class SalesPaymentRecords: BaseEntity<int>
    {
        public int MemoNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public virtual Sales Sales { get; set; } = new Sales();
    }
}
