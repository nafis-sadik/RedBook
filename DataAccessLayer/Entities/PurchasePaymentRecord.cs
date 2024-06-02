using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data.Entities;

public partial class PurchasePaymentRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PurchasePaymentId { get; set; }

    [ForeignKey("Invoice")]
    public int InvoiceId { get; set; }

    [ForeignKey("BankAccount")]
    public int? AccountId { get; set; }

    public DateTime PaymentDate { get; set; }

    public decimal PaymentAmount { get; set; }

    public string TransactionReference { get; set; }

    public string Remarks { get; set; }

    public virtual PurchaseInvoice Invoice { get; set; }

    public virtual BankAccount BankAccount { get; set; }
}
