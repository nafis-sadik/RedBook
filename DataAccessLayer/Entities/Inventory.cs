using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory.Domain.Entities
{
    public partial class Inventory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string ChalanNumber { get; set; }
        public int QuantityAttributeId { get; set; }
        public int OrganizationId { get; set; }

        public virtual Purchase ChalanNumberNavigation { get; set; }
        public virtual Product Product { get; set; }
    }
}
