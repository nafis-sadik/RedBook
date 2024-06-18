using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data.Entities;

public partial class Sale
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SalesId { get; set; }

    [ForeignKey("Invoice")]
    public int InvoiceId { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }

    [ForeignKey("PurchaseInvoice")]
    public int PurchaseId { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int CreateBy { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual SalesInvoice Invoice { get; set; }

    public virtual PurchaseInvoice PurchaseInvoice { get; set; }

    public virtual Product Product { get; set; }
}
