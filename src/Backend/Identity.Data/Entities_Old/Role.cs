﻿using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; }

    public short IsAdminRole { get; set; }

    public int OrganizationId { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual ICollection<RoleRouteMapping> RoleRouteMappings { get; set; } = new List<RoleRouteMapping>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
