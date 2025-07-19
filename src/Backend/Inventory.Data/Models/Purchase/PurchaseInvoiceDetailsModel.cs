namespace Inventory.Data.Models.Purchase
{
    public class PurchaseInvoiceDetailsModel
    {
        /// <summary>
        /// The unique identifier for the purchase details.
        /// </summary>
        public int RecordId { get; set; } = 0;

        /// <summary>
        /// The unique identifier for the purchase invoice.
        /// </summary>
        public int InvoiceId { get; set; } = 0;

        /// <summary>
        /// The unique identifier for the product associated with the purchase details.
        /// </summary>
        public int ProductVariantId { get; set; } = 0;

        /// <summary>
        /// GUID stored as string to track, could be used as Barcode or QR code.
        /// </summary>
        public string BarCode { get; set; } = string.Empty;

        /// <summary>
        /// The name of the product associated with the purchase details.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// The name of the product variant with the purchase details.
        /// </summary>
        public string ProductVariantName { get; set; } = string.Empty;

        /// <summary>
        /// The quantity of the product associated with the purchase details.
        /// </summary>
        public decimal Quantity { get; set; } = 0;

        /// <summary>
        /// The unit price of the product associated with the purchase details.
        /// </summary>
        public decimal PurchasePrice { get; set; } = 0;

        /// <summary>
        /// The discount on the product details.
        /// </summary>
        public decimal PurchaseDiscount { get; set; } = 0;

        /// <summary>
        /// The unit retail price of the product variant.
        /// </summary>
        public decimal RetailPrice { get; set; } = 0;

        /// <summary>
        /// The unit retail price of the product variant.
        /// </summary>
        public decimal MaxRetailDiscount { get; set; } = 0;

        /// <summary>
        /// VAT percentage on product.
        /// </summary>
        public decimal VatRate { get; set; } = 0;

        /// <summary>
        /// The total price of the product details. (Considering unit price and quantity)
        /// </summary>
        public decimal TotalPrice { get; set; } = 0;
        public DateTime? PurchaseDate { get; set; } = null;
    }
}
