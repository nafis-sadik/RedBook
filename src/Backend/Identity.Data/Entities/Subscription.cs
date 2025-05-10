using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Data.Entities;

public partial class Subscription
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SubscriptionId { get; set; }

    public int PackageId { get; set; }

    public int OrganizationId { get; set; }

    public decimal SubscriptionFee { get; set; }

    public DateTime SubscriptionStartDate { get; set; }

    public DateTime CurrentExpiryDate { get; set; }

    [ForeignKey("OrganizationId")]
    public virtual Organization Organization { get; set; }

    [ForeignKey("PackageId")]
    public virtual SubscriptionPackage Package { get; set; }

    public virtual ICollection<SubscriptionTransaction> SubscriptionTransactions { get; set; } = new List<SubscriptionTransaction>();
}
