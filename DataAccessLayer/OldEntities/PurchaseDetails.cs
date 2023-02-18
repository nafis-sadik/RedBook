using RedBook.Core.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class PurchaseDetails: BaseEntity<int>
    {
        public int ChalanNumber { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        //public virtual Purchase Purchase { get; set; } = new Purchase();
    }
}
