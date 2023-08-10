using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class BankBranch
{
    public int BranchId { get; set; }

    public int BankId { get; set; }

    public string BranchName { get; set; }

    public virtual Bank Bank { get; set; }

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
}
