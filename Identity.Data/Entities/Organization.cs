﻿namespace Identity.Data.Entities;

public partial class Organization
{
    public int OrganizationId { get; set; }

    public string OrganizationName { get; set; }

    public virtual ICollection<OrganizationRoleMapping> OrganizationRoleMappings { get; set; } = new List<OrganizationRoleMapping>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
