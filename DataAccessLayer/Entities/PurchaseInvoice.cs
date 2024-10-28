using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Inventory.Data.Entities;

public partial class PurchaseInvoice
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int InvoiceId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public decimal GrossTotal { get; set; }

    public decimal TotalDiscount { get; set; }

    public int OrganizationId { get; set; }

    public string ChalanNumber { get; set; }

    public DateTime ChalanDate { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string Terms { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string Remarks { get; set; }

    [AllowNull]
    [ForeignKey("Vendor")]
    public int? VendorId { get; set; }

    public int CreateBy { get; set; }

    public DateTime CreateDate { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<PurchasePaymentRecord> PurchasePaymentRecords { get; set; } = new List<PurchasePaymentRecord>();

    public virtual ICollection<PurchaseInvoiceDetails> Purchases { get; set; } = new List<PurchaseInvoiceDetails>();

    public virtual Vendor? Vendor { get; set; }
}
