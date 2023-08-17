using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class Route
{
    public int RouteId { get; set; }

    public string RouteName { get; set; }

    public string Route1 { get; set; }

    public string Description { get; set; }

    public int ApplicationId { get; set; }

    public virtual Application Application { get; set; }

    public virtual ICollection<RoleRouteMapping> RoleRouteMappings { get; set; } = new List<RoleRouteMapping>();
}
