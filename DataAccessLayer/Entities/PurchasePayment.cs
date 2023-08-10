using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class PurchasePayment
{
    public int PurchasePaymentId { get; set; }

    public int PurchaseId { get; set; }

    public DateTime PaymentDate { get; set; }

    public decimal PaymentAmount { get; set; }

    public virtual Purchase Purchase { get; set; }
}
