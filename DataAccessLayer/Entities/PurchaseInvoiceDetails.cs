using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data.Entities;

[Table("PurchaseInvoiceDetails")]
public partial class PurchaseInvoiceDetails
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RecordId { get; set; }

    public int InvoiceId { get; set; }

    public int ProductVariantId { get; set; }

    public string ProductName { get; set; }

    public string ProductVariantName { get; set; }

    public string BarCode { get; set; }

    public decimal Quantity { get; set; }

    public decimal PurchasePrice { get; set; }

    public decimal PurchaseDiscount { get; set; }

    public decimal RetailPrice { get; set; }

    public decimal MaxRetailDiscount { get; set; }

    public decimal VatRate { get; set; }

    public decimal CurrentStockQuantity { get; set; }

    [Required]
    public int CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    [ForeignKey("InvoiceId")]
    public virtual PurchaseInvoice Invoice { get; set; }

    [ForeignKey("ProductVariantId")]
    public virtual ProductVariant ProductVariant { get; set; }
}
