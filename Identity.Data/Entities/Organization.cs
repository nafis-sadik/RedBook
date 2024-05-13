using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class Organization
{
    public int OrganizationId { get; set; }

    public string OrganizationName { get; set; }

    public string Address { get; set; }

    public string LogoUrl { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string UpdatededBy { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
