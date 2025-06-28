import { ProductVariantModel } from "./product-variant.model";
import { PurchaseInvoiceDetailsModel } from "./purchase-invoice-details.model";

export class SalesInvoiceDetailsModel {
    /**
     * The unique identifier for the sales invoice details.
     * @type {number}
     */
    recordId: number = 0;
    /**
     * The unique identifier for the sales invoice.
     * @type {number}
     */
    invoiceId: number = 0;
    /**
     * The unique identifier for the product selected on UI. (Required for viewing on UI, not necessary for API)
     * @type {number}
     */
    productId: number = 0;
    /**
     * The unique identifier for the variant of the selected product that is being sold under this invoice.
     * @type {number}
     */
    productVariantId: number = 0;
    /**
     * The unique identifier for the variant of the selected product that is going to be generated in the backend and used to track inventory and other details per lot.
     * @type {string}
     */
    barCode: string = '';
    /**
     * Selected product name (Primarily for display purpose).
     * @type {string}
     */
    productName: string = '';
    /**
     * Selected variant name of the selected product (Primarily for display purpose).
     * @type {string}
     */
    productVariantName: string = '';
    /**
     * The quantity of the variant sold under this invoice.
     * @type {number}
     */
    quantity: number = 0;
    /**
     * The quantity of the product associated with the purchase details.
     * @type {number}
     */
    maxQuantity: number = 0;
    /**
     * Retail price of this product
     * @type {number}
     */
    maxRetailPrice: number = 0;
    /**
     * Retail price of this product
     * @type {number}
     */
    minRetailPrice: number = 0;
    /**
     * Retail price of this product
     * @type {number}
     */
    retailPrice: number = 0;
    /**
     * Maximum amount of retail discount on this product for this lot
     * @type {number}
     */
    retailDiscount: number = 0;
    /**
     * The total price of the product details. (Considering unit price and quantity)
     * @type {number}
     */
    vatRate: number = 0;
    /**
     * The total price of the product details. (Considering unit price and quantity)
     * @type {number}
     */
    totalCostPrice: number = 0;
    /**
     * Purchase Invoice Id of this variant
     * @type {number}
     */
    lotId: number = 0;    
    /**
     * Purchase Invoice object of this variant
     * @type {PurchaseInvoiceDetailsModel}
     */
    lot: PurchaseInvoiceDetailsModel = new PurchaseInvoiceDetailsModel()
    /**
     * Array of product variants of the selected product. (Required for viewing on UI, not necessary for API)
     */
    variants: Array<ProductVariantModel> = [];
}