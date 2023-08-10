using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class BankAccount
{
    public int AccountId { get; set; }

    public string AccountName { get; set; }

    public int BranchId { get; set; }

    public virtual BankBranch Branch { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
