using Inventory.Data.Entities;

namespace Inventory.Data.Models.Purchase
{
    public class InvoiceModel
    {
        public int InvoiceId { get; set; }

        public DateTime ChalanDate { get; set; }

        public decimal GrossTotal { get; set; }

        public decimal TotalDiscount { get; set; }

        public int OrganizationId { get; set; }

        public string ChalanNumber { get; set; }

        public string Remarks { get; set; }

        public string Terms { get; set; }

        public int? VendorId { get; set; }

        public int CreateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public List<PurchaseInvoiceDetails> PurchaseDetails { get; set; } = new List<PurchaseInvoiceDetails>();
    }
}
