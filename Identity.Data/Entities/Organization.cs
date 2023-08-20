using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class Organization
{
    public int OrganizationId { get; set; }

    public string OrganizationName { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
