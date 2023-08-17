using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public decimal Quantity { get; set; }

    public int QuantityAttributeId { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? UpdateBy { get; set; }

    public int OrganizationId { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; }

    public virtual UserCache CreateByNavigation { get; set; }

    public virtual OrganizationCache Organization { get; set; }

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual CommonAttribute QuantityAttribute { get; set; }

    public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();

    public virtual UserCache UpdateByNavigation { get; set; }
}
