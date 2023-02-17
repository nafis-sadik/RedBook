using RedBook.Core.EntityFramework;

namespace Inventory.Domain.Entities
{
    public class Inventory : BaseEntity<int>
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public int ChalanNumber { get; set; }
        public int QuantityAttributeId { get; set; }
        public virtual CommonAttribute QuantityAttribute { get; set; } = new CommonAttribute();
    }
}
