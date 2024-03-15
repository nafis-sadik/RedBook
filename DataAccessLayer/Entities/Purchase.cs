using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public int ProductId { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int InvoiceId { get; set; }

    public virtual PurchaseInvoice Invoice { get; set; }

    public virtual Product Product { get; set; }
}
