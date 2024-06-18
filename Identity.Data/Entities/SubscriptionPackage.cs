using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Data.Entities;

public partial class SubscriptionPackage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PackageId { get; set; }

    public string PackageName { get; set; }

    public int ApplicationId { get; set; }

    public decimal? SubscriptionFee { get; set; }

    public string PackageDescription { get; set; }

    [ForeignKey("ApplicationId")]
    public virtual Application Application { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
