using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class RoleRouteMapping
{
    public int MappingId { get; set; }

    public int RouteId { get; set; }

    public int RoleId { get; set; }

    public virtual Role Role { get; set; }

    public virtual Route Route { get; set; }
}
