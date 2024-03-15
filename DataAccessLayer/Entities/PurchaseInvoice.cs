using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class PurchaseInvoice
{
    public int InvoiceId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public int? PurchasedBy { get; set; }

    public string CheckNumber { get; set; }

    public decimal TotalPurchasePrice { get; set; }

    public int OrganizationId { get; set; }

    public string ChalanNumber { get; set; }

    public int PurchaseId { get; set; }

    public int CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<PurchasePaymentRecord> PurchasePaymentRecords { get; set; } = new List<PurchasePaymentRecord>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
