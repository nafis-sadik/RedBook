using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory.Data.Entities
{
    public partial class Product
    {
        public Product()
        {
            Inventories = new HashSet<Inventory>();
            PurchaseDetails = new HashSet<PurchaseDetail>();
            SalesDetails = new HashSet<SalesDetail>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public int QuantityAttributeId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public int OrganizationId { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual ICollection<SalesDetail> SalesDetails { get; set; }
    }
}
