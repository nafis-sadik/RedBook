using Inventory.Data.Models.Purchase;

namespace Inventory.Data.Models.Sales
{
    public class SalesInvoiceDetailsModel
    {
        /// <summary>
        /// The unique identifier for the sales invoice details.
        /// </summary>
        public int RecordId { get; set; } = 0;

        /// <summary>
        /// The unique identifier for the sales invoice.
        /// </summary>
        public int InvoiceId { get; set; } = 0;

        /// <summary>
        /// The unique identifier for the product selected on UI. (Required for viewing on UI, not necessary for API)
        /// </summary>
        public int ProductId { get; set; } = 0;

        /// <summary>
        /// The unique identifier for the variant of the selected product that is being sold under this invoice.
        /// </summary>
        public int ProductVariantId { get; set; } = 0;

        /// <summary>
        /// The unique identifier for the variant of the selected product that is going to be generated in the backend and used to track inventory and other details per lot.
        /// </summary>
        public string BarCode { get; set; } = string.Empty;

        /// <summary>
        /// Selected product name (Primarily for display purpose).
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Selected variant name of the selected product (Primarily for display purpose).
        /// </summary>
        public string ProductVariantName { get; set; } = string.Empty;

        /// <summary>
        /// The quantity of the variant sold under this invoice.
        /// </summary>
        public decimal Quantity { get; set; } = 0;

        /// <summary>
        /// The quantity of the product associated with the purchase details.
        /// </summary>
        public int MaxQuantity { get; set; } = 0;

        /// <summary>
        /// Retail price of this product
        /// </summary>
        public decimal MaxRetailPrice { get; set; } = 0;

        /// <summary>
        /// Retail price of this product
        /// </summary>
        public decimal MinRetailPrice { get; set; } = 0;

        /// <summary>
        /// Retail price of this product
        /// </summary>
        public decimal RetailPrice { get; set; } = 0;

        /// <summary>
        /// Maximum amount of retail discount on this product for this lot
        /// </summary>
        public decimal RetailDiscount { get; set; } = 0;

        /// <summary>
        /// Vat rate applicable on this product
        /// </summary>
        public decimal VatRate { get; set; } = 0;

        /// <summary>
        /// The total price of the product details. (Considering unit price and quantity)
        /// </summary>
        public decimal TotalCostPrice { get; set; } = 0;

        /// <summary>
        /// Purchase Invoice Id of this variant
        /// </summary>
        public int LotId { get; set; } = 0;

        /// <summary>
        /// Purchase Invoice Id of this variant
        /// </summary>
        public PurchaseInvoiceDetailsModel? Lot { get; set; }
    }
}
