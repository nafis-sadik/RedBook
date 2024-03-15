using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class PurchasePaymentRecord
{
    public int PurchasePaymentId { get; set; }

    public int InvoiceId { get; set; }

    public DateTime PaymentDate { get; set; }

    public decimal PaymentAmount { get; set; }

    public virtual PurchaseInvoice Invoice { get; set; }
}
