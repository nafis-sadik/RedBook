import { PurchaseInvoiceDetailsModel } from "./purchase-invoice-details.model";
import { PurchasePaymentModel } from "./purchase-payment.model";

export class PurchaseInvoiceModel {
  /**
   * The unique identifier for the purchase invoice.
   * @type {number}
   */
  invoiceId: number = 0;

  /**
   * Purchase date for this invoice.
   * @type {string}
   */
  chalanDate: string = '';

  /**
   * Person or entity, you are purchasing from.
   * @type {string}
   */
  vendorName: string = '';

  /**
   * The total purchase price for the invoice.
   * @type {number}
   */
  invoiceTotal: number = 0;

  /**
   * The total amount of mmoney paid to this vendor against this invoice.
   * @type {number}
   */
  totalPaid: number = 0;

  /**
   * The total discount value on the invoice.
   * @type {number}
   */
  totalDiscount: number = 0;

  /**
   * Current status of payment for this invoice
   * @type {string}
   */
  paymentStatus: string = '';

  /**
   * The unique identifier for the organization associated with the purchase invoice.
   * @type {number}
   */
  organizationId: number = 0;

  /**
   * The unique identifier for the purchase invoice.
   * @type {string}
   */
  chalanNumber: string = '';

  /**
   * The remarks associated with the purchase invoice.
   * @type {string}
   */
  remarks: string = '';

  /**
   * The terms and conditions associated with the purchase invoice.
   * @type {string}
   */
  terms: string = '';

  /**
   * The unique identifier for the vendor associated with the purchase invoice.
   * @type {number}
   */
  vendorId: number = 0;

  /**
   * An array of purchase details associated with the purchase invoice.
   * @type {Array<PurchaseInvoiceDetailsModel>}
   */
  purchaseDetails: Array<PurchaseInvoiceDetailsModel> = [];

  /**
   * Payment records associated with the purchase invoice.
   * @type {Array<PurchasePaymentModel>}
   */
  paymentRecords: Array<PurchasePaymentModel> = [];
}
