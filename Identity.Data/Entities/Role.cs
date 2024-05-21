using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; }

    public int? OrganizationId { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsRetailer { get; set; }

    public bool IsSystemAdmin { get; set; }

    public bool IsOwner { get; set; }

    public int? ApplicationId { get; set; }

    public virtual Application Application { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual ICollection<RoleRouteMapping> RoleRouteMappings { get; set; } = new List<RoleRouteMapping>();

    public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; } = new List<UserRoleMapping>();
}
