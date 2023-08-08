﻿namespace Identity.Data.Entities;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; }

    public short IsGenericRole { get; set; }

    public virtual ICollection<OrganizationRoleMapping> OrganizationRoleMappings { get; set; } = new List<OrganizationRoleMapping>();

    public virtual ICollection<RoleRouteMapping> RoleRouteMappings { get; set; } = new List<RoleRouteMapping>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
