using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class OrganizationRoleMapping
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int OrganizationId { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual Role Role { get; set; }
}
