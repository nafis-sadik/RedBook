#nullable disable

namespace Inventory.Domain.Entities
{
    public partial class Bank
    {
        public Bank()
        {
            BankBranches = new HashSet<BankBranch>();
            PurchaseFromBanks = new HashSet<Purchase>();
            PurchaseToBanks = new HashSet<Purchase>();
        }

        public int Id { get; set; }
        public string BankName { get; set; }

        public virtual ICollection<BankBranch> BankBranches { get; set; }
        public virtual ICollection<Purchase> PurchaseFromBanks { get; set; }
        public virtual ICollection<Purchase> PurchaseToBanks { get; set; }
    }
}
