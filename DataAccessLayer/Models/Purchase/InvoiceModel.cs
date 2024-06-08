namespace Inventory.Data.Models.Purchase
{
    public class InvoiceModel
    {
        public int InvoiceId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public decimal TotalPurchasePrice { get; set; }

        public int OrganizationId { get; set; }

        public string ChalanNumber { get; set; }

        public string Remarks { get; set; }

        public int? VendorId { get; set; }

        public int CreateBy { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
