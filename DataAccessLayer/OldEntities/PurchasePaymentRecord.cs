using RedBook.Core.EntityFramework;

namespace Inventory.Domain.Entities
{
    public class PurchasePaymentRecord: BaseEntity<int>
    {
        public int ChalanNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        //public virtual Purchase Purchase { get; set; } = new Purchase();
    }
}
