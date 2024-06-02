using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data.Entities;

public partial class SalesPaymentRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SalesPaymentId { get; set; }

    [ForeignKey("SalesInvoice")]
    public int InvoiceId { get; set; }

    public int? PaidBy { get; set; }

    public DateTime PaymentDate { get; set; }

    public decimal PaymentAmount { get; set; }

    public virtual SalesInvoice Invoice { get; set; }
}
