using RedBook.Core.Models;

namespace Inventory.Data.Models
{
    public class PurchaseInvoiceModel: PagedModel<ProductModel>
    {
        public int InvoiceId { get; set; }

        public int PurchaseId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public int? PurchasedBy { get; set; }

        public string CheckNumber { get; set; }

        public decimal TotalPurchasePrice { get; set; }

        public int OrganizationId { get; set; }

        public string ChalanNumber { get; set; }
    }
}
