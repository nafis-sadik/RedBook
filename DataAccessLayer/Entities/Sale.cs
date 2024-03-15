using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class Sale
{
    public int SalesId { get; set; }

    public int InvoiceId { get; set; }

    public int ProductId { get; set; }

    public string ChalanNo { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int CreateBy { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual SalesInvoice Invoice { get; set; }

    public virtual Product Product { get; set; }
}
