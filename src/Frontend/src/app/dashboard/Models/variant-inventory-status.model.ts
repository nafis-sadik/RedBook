import { PurchaseInvoiceDetailsModel } from "./purchase-invoice-details.model";

export class VariantInventoryStatusModel {
    productId: number = 0;
    variantId: number = 0;
    productName: string = '';
    variantName: string = '';
    productCategory: string = '';
    productSubCategory: string = '';
    productImage: string = '';
    sku: string = '';
    totalStockQuantity: number = 0;
    lots: Array<PurchaseInvoiceDetailsModel> = [];
}