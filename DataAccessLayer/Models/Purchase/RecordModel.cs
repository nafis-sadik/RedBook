using Inventory.Data.Entities;

namespace Inventory.Data.Models.Purchase
{
    public class RecordModel
    {
        public int PurchaseId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public int PurchasedBy { get; set; }

        public int AccountId { get; set; }

        public string CheckNumber { get; set; }

        public decimal TotalPurchasePrice { get; set; }

        public int OrganizationId { get; set; }

        public string ChalanNumber { get; set; }

        public virtual BankAccount Account { get; set; }

    }
}
