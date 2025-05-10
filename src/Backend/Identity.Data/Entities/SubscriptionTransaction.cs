using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Data.Entities;

public partial class SubscriptionTransaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionId { get; set; }

    public int SubscriptionId { get; set; }

    public int PaidAmount { get; set; }

    public DateTime PaymentDate { get; set; }

    public int PaidBy { get; set; }

    [ForeignKey("PaidBy")]
    public virtual User PaidByNavigation { get; set; }

    [ForeignKey("SubscriptionId")]
    public virtual Subscription Subscription { get; set; }
}
