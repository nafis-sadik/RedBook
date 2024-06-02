using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data.Entities;

public partial class PurchaseInvoice
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int InvoiceId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public decimal TotalPurchasePrice { get; set; }

    public int OrganizationId { get; set; }

    public string ChalanNumber { get; set; }

    public string Remarks { get; set; }

    [ForeignKey("Vendor")]
    public int VendorId { get; set; }

    public int CreateBy { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ICollection<PurchasePaymentRecord> PurchasePaymentRecords { get; set; } = new List<PurchasePaymentRecord>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual Vendor Vendor { get; set; }
}
