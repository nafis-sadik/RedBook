import { ProductVariantModel } from "./product-variant.model";

export class ProductModel{
    /**
     * Product Id
     * @type {number}
     */
    productId: number = 0;
    /**
     * Product Name
     * @type {string}
     */
    productName: string;
    /**
     * Product Category or Subcategory Id
     * @type {number}
     */
    categoryId: number = 0;
    /**
     * Product category name
     * @type {string}
     */
    categoryName: string;
    /**
     * Product subcategory or Subcategory Id
     * @type {number}
     */
    subcategoryId: number = 0;
    /**
     * Product subcategory name
     * @type {string}
     */
    subcategoryName: string;
    /**
     * Product purchase price
     * @type {number}
     */
    purchasePrice: number = 0;
    /**
     * Product retail price
     * @type {number}
     */
    retailPrice: number = 0;
    /**
     * Organization Unique Identifier
     * @type {number}
     */
    organizationId: number = 0;
    /**
     * Purchase/Sales/Inventory Quantity
     * @type {number}
     */
    quantity: number = 0;
    /**
     * Quantity Type Attribute (i.e. Kg, Bottle, Packets, Liter etc)
     * @type {number}
     */
    quantityTypeId: number = 0;
    /**
     * Brand id of product, (i.e. RFL, Beximco, GE etc)
     * @type {number}
     */
    brandId: number = 0;
    /**
     * Brand name of product, (i.e. RFL, Beximco, GE etc)
     * @type {string}
     */
    brandName: string = '';
    productVariants: Array<ProductVariantModel> = [];
}
