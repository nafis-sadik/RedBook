using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory.Data.Entities
{
    public partial class PurchaseDetail
    {
        public int Id { get; set; }
        public string ChalanNumber { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual Purchase ChalanNumberNavigation { get; set; }
        public virtual Product Product { get; set; }
    }
}
