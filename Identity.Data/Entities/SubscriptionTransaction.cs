using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class SubscriptionTransaction
{
    public int TransactionId { get; set; }

    public int SubscriptionId { get; set; }

    public int PaidAmount { get; set; }

    public DateTime PaymentDate { get; set; }

    public int PaidBy { get; set; }

    public virtual User PaidByNavigation { get; set; }

    public virtual Subscription Subscription { get; set; }
}
