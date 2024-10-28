using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int ProductId { get; set; } = 0;

        /// <summary>
        /// The name of the product associated with the purchase details.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// The quantity of the product associated with the purchase details.
        /// </summary>
        public int Quantity { get; set; } = 0;

        /// <summary>
        /// The unit price of the product associated with the purchase details.
        /// </summary>
        public decimal UnitPrice { get; set; } = 0;

        /// <summary>
        /// The discount on the product details.
        /// </summary>
        public decimal Discount { get; set; } = 0;

        /// <summary>
        /// VAT percentage on product.
        /// </summary>
        public decimal Vat { get; set; } = 0;

        /// <summary>
        /// The total price of the product details. (Considering unit price and quantity)
        /// </summary>
        public decimal TotalPrice { get; set; } = 0;
    }
}
