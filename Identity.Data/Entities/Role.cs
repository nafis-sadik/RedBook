using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Identity.Data.Entities;

public partial class Role
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoleId { get; set; }

    public string RoleName { get; set; }

    [AllowNull]
    public int? OrganizationId { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsRetailer { get; set; }

    public bool IsSystemAdmin { get; set; }

    public bool IsOwner { get; set; }

    [AllowNull]
    public int? ApplicationId { get; set; }

    [ForeignKey("ApplicationId")]
    public virtual Application Application { get; set; }

    [ForeignKey("OrganizationId")]
    public virtual Organization Organization { get; set; }

    public virtual ICollection<RoleRouteMapping> RoleRouteMappings { get; set; } = new List<RoleRouteMapping>();

    public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; } = new List<UserRoleMapping>();
}
