export class ProductVariantModel{
    /**
     * Variant Id
     * @type {number}
     */
    variantId: number;

    /**
     * Variant Name
     * @type {string}
     */
    variantName: string;

    /**
     * Quantity of the variant currently available in stock
     * @type {number}
     */
    stockQuantity: number;
    
    /**
     * Stock Keeping Unit
     * @type {string}
     */
    sku: string = '';
    
    /**
     * Attributes of the variant
     * @type {string}
     */
    attributes: string = '';

    /**
     * Product Id of the product this variant belongs to
     * @type {number}
     */
    productId: number;
}