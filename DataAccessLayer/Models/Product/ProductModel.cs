namespace Inventory.Data.Models.Product
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int OrganizationId { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int SubcategoryId { get; set; }

        public string SubcategoryName { get; set; }

        public int BrandId { get; set; }

        public string BrandName { get; set; }

        public float PurchasePrice { get; set; }

        public float RetailPrice { get; set; }

        public int Quantity { get; set; }

        public int QuantityTypeId { get; set; }

        public IEnumerable<ProductVariantModel> ProductVariants { get; set; }
    }
}
