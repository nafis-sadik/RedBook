using Inventory.Data.Models.Purchase;

namespace Inventory.Data.Models.Product
{
    public class VariantInventoryStatusModel
    {
        public int ProductId { get; set; }
        public int VariantId { get; set; }
        public string ProductName { get; set; }
        public string VariantName { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductImage { get; set; }
        public string SKU { get; set; }
        public IList<PurchaseInvoiceDetailsModel> Lots { get; set; }
    }
}
