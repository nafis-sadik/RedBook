using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data.Entities;

public partial class SalesInvoice
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int InvoiceId { get; set; }

    public decimal InvoiceTotal { get; set; }

    public decimal TotalDiscount { get; set; }

    public int OrganizationId { get; set; }

    public string InvoiceNumber { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string Terms { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string Remarks { get; set; }

    public int CreateBy { get; set; }

    public DateTime CreateDate { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("Customer")]
    public int? CustomerId { get; set; } = null;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<SalesInvoiceDetails> SalesInvoiceDetails { get; set; } = new List<SalesInvoiceDetails>();

    public virtual ICollection<SalesPaymentRecord> SalesPaymentRecords { get; set; } = new List<SalesPaymentRecord>();
}