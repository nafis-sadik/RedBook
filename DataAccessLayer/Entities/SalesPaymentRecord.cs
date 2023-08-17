using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class SalesPaymentRecord
{
    public int SalesPaymentId { get; set; }

    public int SalesId { get; set; }

    public DateTime PaymentDate { get; set; }

    public decimal PaymentAmount { get; set; }

    public virtual Sale Sales { get; set; }
}
