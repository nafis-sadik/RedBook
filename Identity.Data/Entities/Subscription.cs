using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class Subscription
{
    public int SubscriptionId { get; set; }

    public int PackageId { get; set; }

    public int OrganizationId { get; set; }

    public decimal SubscriptionFee { get; set; }

    public DateTime SubscriptionStartDate { get; set; }

    public DateTime CurrentExpiryDate { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual SubscriptionPackage Package { get; set; }

    public virtual ICollection<SubscriptionTransaction> SubscriptionTransactions { get; set; } = new List<SubscriptionTransaction>();
}
