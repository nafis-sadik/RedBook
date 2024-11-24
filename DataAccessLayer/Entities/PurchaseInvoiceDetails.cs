using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data.Entities;

[Table("PurchaseInvoiceDetails")]
public partial class PurchaseInvoiceDetails
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RecordId { get; set; }

    public int? ProductId { get; set; }

    public int InvoiceId { get; set; }

    public string ProductName { get; set; }

    public decimal Quantity { get; set; }

    public decimal PurchasePrice { get; set; }

    public decimal RetailPrice { get; set; }

    public decimal VatRate { get; set; }

    public decimal Discount { get; set; }

    [Required]
    public int CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    [ForeignKey("InvoiceId")]
    public virtual PurchaseInvoice Invoice { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }
}
