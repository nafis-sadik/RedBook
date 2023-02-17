using RedBook.Core.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Domain.Entities
{
    // Primary key from BaseEntity shall work as Chalan Number
    public class Purchase: BaseEntity<int>
    {
        public DateTime PurchaseDate { get; set; }
        public Guid PurchasedBy { get; set; }
        public string AccountNumber { get; set; }

        public int ToBankId { get; set; }
        public virtual Bank ToBank { get; set; } = new Bank();
        public int ToBankBranchId { get; set; }
        public virtual BankBranches ToBankBranch { get; set; } = new BankBranches();

        public int FromBankId { get; set; }
        public virtual Bank FromBank { get; set; } = new Bank();
        public int FromBankBranchId { get; set; }
        public virtual BankBranches FromBankBranch { get; set; } = new BankBranches();
        [MaxLength(200)]
        [Column(TypeName = "varchar(200)")]
        public string CheckNumber { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public virtual ICollection<PurchaseDetails> PurchaseDetails { get; set; } = new List<PurchaseDetails>();
        public virtual ICollection<PurchasePaymentRecord> PurchasePaymentRecord { get; set; } = new List<PurchasePaymentRecord>();
    }
}
