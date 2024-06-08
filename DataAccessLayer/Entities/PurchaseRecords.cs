using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data.Entities;

public partial class PurchaseRecords
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PurchaseId { get; set; }

    public int ProductId { get; set; }

    public int ProductName { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int InvoiceId { get; set; }

    public int? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    [ForeignKey("InvoiceId")]
    public virtual PurchaseInvoice Invoice { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }
}
