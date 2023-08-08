using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class Role
{
    public int Id { get; set; }

    public string RoleName { get; set; }

    public bool? IsGenericRole { get; set; }

    public virtual ICollection<OrganizationRoleMapping> OrganizationRoleMappings { get; set; } = new List<OrganizationRoleMapping>();

    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
