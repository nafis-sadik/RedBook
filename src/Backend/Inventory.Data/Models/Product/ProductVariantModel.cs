namespace Inventory.Data.Models.Product
{
    public class ProductVariantModel
    {
        public int VariantId { get; set; }

        public string VariantName { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string SKU { get; set; }

        public string BarCode { get; set; }

        public string Attributes { get; set; }

        public decimal StockQuantity { get; set; }

        public float RetailPrice { get; set; }

        public float MaxDiscount { get; set; }
    }
}
