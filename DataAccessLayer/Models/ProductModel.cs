namespace Inventory.Data.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int CategoryId { get; set; }

        public int SubcategoryId { get; set; }

        public string CategoryName { get; set; }

        public string SubcategoryName { get; set; }

        public float PurchasePrice { get; set; }

        public float RetailPrice { get; set; }

        public int OrganizationId { get; set; }

        public int Quantity { get; set; }

    }
}
