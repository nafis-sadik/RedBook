using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class Organization
{
    public int Id { get; set; }

    public string OrganizationName { get; set; }

    public virtual ICollection<OrganizationRoleMapping> OrganizationRoleMappings { get; set; } = new List<OrganizationRoleMapping>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
