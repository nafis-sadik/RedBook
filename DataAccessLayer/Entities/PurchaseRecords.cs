using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data.Entities;

public partial class PurchaseRecords
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RecordId { get; set; }

    [AllowedValues(true)]
    public int? ProductId { get; set; }

    public int ProductName { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int InvoiceId { get; set; }

    [Required]
    public int CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    [ForeignKey("InvoiceId")]
    public virtual PurchaseInvoice Invoice { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }
}
