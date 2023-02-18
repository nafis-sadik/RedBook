using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory.Domain.Entities
{
    public partial class BankBranch
    {
        public BankBranch()
        {
            PurchaseFromBankBranches = new HashSet<Purchase>();
            PurchaseToBankBranches = new HashSet<Purchase>();
        }

        public int Id { get; set; }
        public int BankId { get; set; }
        public string BranchName { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual ICollection<Purchase> PurchaseFromBankBranches { get; set; }
        public virtual ICollection<Purchase> PurchaseToBankBranches { get; set; }
    }
}
