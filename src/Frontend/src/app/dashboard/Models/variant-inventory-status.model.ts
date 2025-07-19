import { PurchaseInvoiceDetailsModel } from "./purchase-invoice-details.model";

export class VariantInventoryStatusModel {
    productId: number = 0;
    variantId: number = 0;
    productName: string = '';
    variantName: string = '';
    sku: string = '';
    lots: Array<PurchaseInvoiceDetailsModel> = [];
}