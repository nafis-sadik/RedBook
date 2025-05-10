using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data.Entities;

[Table("SalesInvoiceDetails")]
public partial class SalesInvoiceDetails
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RecordId { get; set; }

    [ForeignKey("Invoice")]
    public int InvoiceId { get; set; }

    public string ProductName { get; set; }

    public string ProductVariantName { get; set; }

    [ForeignKey("Variant")]
    public int VariantId { get; set; }

    [ForeignKey("PurchaseInvoice")]
    public int PurchaseId { get; set; }

    public decimal Quantity { get; set; }

    public decimal RetailPrice { get; set; }

    public decimal RetailDiscount { get; set; }

    public decimal VatRate { get; set; }

    public decimal VatAmount { get; set; }

    [Required]
    public int CreateBy { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual SalesInvoice Invoice { get; set; }

    public virtual PurchaseInvoice PurchaseInvoice { get; set; }

    public virtual ProductVariant Variant { get; set; }
}
