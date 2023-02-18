using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory.Domain.Entities
{
    public partial class Purchase
    {
        public Purchase()
        {
            Inventories = new HashSet<Inventory>();
            PurchaseDetails = new HashSet<PurchaseDetail>();
            PurchasePaymentRecords = new HashSet<PurchasePaymentRecord>();
            SalesDetails = new HashSet<SalesDetail>();
        }

        public string Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchasedBy { get; set; }
        public string AccountNumber { get; set; }
        public int ToBankId { get; set; }
        public int ToBankBranchId { get; set; }
        public int FromBankId { get; set; }
        public int FromBankBranchId { get; set; }
        public string CheckNumber { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public int OrganizationId { get; set; }

        public virtual Bank FromBank { get; set; }
        public virtual BankBranch FromBankBranch { get; set; }
        public virtual Bank ToBank { get; set; }
        public virtual BankBranch ToBankBranch { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual ICollection<PurchasePaymentRecord> PurchasePaymentRecords { get; set; }
        public virtual ICollection<SalesDetail> SalesDetails { get; set; }
    }
}
