using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Identity.Data.Entities;

public partial class Route
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RouteId { get; set; }

    public string RouteName { get; set; }

    public string RoutePath { get; set; }

    public string Description { get; set; }

    public int ApplicationId { get; set; }

    [AllowNull]
    public int? ParentRouteId { get; set; }

    public int RouteTypeId { get; set; }

    public bool IsMenuRoute { get; set; }

    [ForeignKey("ParentRouteId")]
    public virtual Route ParentRoute { get; set; }

    [ForeignKey("RouteTypeId")]
    public virtual RouteType RouteType { get; set; }

    [ForeignKey("ApplicationId")]
    public virtual Application Application { get; set; }

    public virtual ICollection<Route> InverseParentRoute { get; set; } = new List<Route>();

    public virtual ICollection<RoleRouteMapping> RoleRouteMappings { get; set; } = new List<RoleRouteMapping>();
}
