using Inventory.Data.Entities;

namespace Inventory.Data.Models.Purchase
{
    public class PurchaseInvoiceModel
    {
        public int InvoiceId { get; set; }

        public DateTime ChalanDate { get; set; }

        public decimal InvoiceTotal { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal TotalPaid { get; set; }

        public decimal TotalDue { get; set; }

        public int OrganizationId { get; set; }

        public string PaymentStatus { get; set; }

        public string Remarks { get; set; }

        public string Terms { get; set; }

        public int? VendorId { get; set; }

        public string VendorName { get; set; }

        public string ChalanNumber { get; set; }

        public int CreateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public List<PurchaseInvoiceDetailsModel> PurchaseDetails { get; set; } = new List<PurchaseInvoiceDetailsModel>();
    }
}