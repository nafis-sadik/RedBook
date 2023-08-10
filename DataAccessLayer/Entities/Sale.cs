using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class Sale
{
    public int SalesId { get; set; }

    public DateTime SalesDate { get; set; }

    public string SoldBy { get; set; }

    public decimal TotalAmount { get; set; }

    public int OrganizationId { get; set; }

    public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();

    public virtual ICollection<SalesPaymentRecord> SalesPaymentRecords { get; set; } = new List<SalesPaymentRecord>();
}
