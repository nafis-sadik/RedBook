using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class PurchaseDetail
{
    public int PurchaseDetailsId { get; set; }

    public int PurchaseId { get; set; }

    public int ProductId { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Product Product { get; set; }

    public virtual Purchase Purchase { get; set; }
}
