using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class SalesPaymentRecord
{
    public int SalesPaymentId { get; set; }

    public int InvoiceId { get; set; }

    public int? PaidBy { get; set; }

    public DateTime PaymentDate { get; set; }

    public decimal PaymentAmount { get; set; }

    public virtual SalesInvoice Invoice { get; set; }
}
