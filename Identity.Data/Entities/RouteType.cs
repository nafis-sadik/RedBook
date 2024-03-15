using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class RouteType
{
    public int RouteTypeId { get; set; }

    public string RouteTypeName { get; set; }

    public virtual ICollection<Route> Routes { get; set; } = new List<Route>();
}
