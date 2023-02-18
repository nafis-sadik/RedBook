using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory.Domain.Entities
{
    public partial class SalesDetail
    {
        public int Id { get; set; }
        public string MemoNumber { get; set; }
        public int ProductId { get; set; }
        public string ChalanNo { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual Purchase ChalanNoNavigation { get; set; }
        public virtual Sale MemoNumberNavigation { get; set; }
        public virtual Product Product { get; set; }
    }
}
