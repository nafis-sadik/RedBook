using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Data.Entities;

public partial class Application
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ApplicationId { get; set; }

    public string ApplicationName { get; set; }

    public string ApplicationUrl { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<Route> Routes { get; set; } = new List<Route>();

    public virtual ICollection<SubscriptionPackage> SubscriptionPackages { get; set; } = new List<SubscriptionPackage>();
}
