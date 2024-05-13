using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class SubscriptionPackage
{
    public int PackageId { get; set; }

    public string PackageName { get; set; }

    public int ApplicationId { get; set; }

    public decimal? SubscriptionFee { get; set; }

    public string PackageDescription { get; set; }

    public virtual Application Application { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
