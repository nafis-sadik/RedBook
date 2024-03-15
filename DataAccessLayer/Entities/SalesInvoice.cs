using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class SalesInvoice
{
    public int InvoiceId { get; set; }

    public DateTime SalesDate { get; set; }

    public string SoldBy { get; set; }

    public decimal TotalAmount { get; set; }

    public int OrganizationId { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<SalesPaymentRecord> SalesPaymentRecords { get; set; } = new List<SalesPaymentRecord>();
}
