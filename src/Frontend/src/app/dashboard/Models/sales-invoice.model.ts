import { CustomerModel } from "./customer.model";
import { SalesInvoiceDetailsModel } from "./sales-invoice-details.model";
import { SalesPaymentModel } from "./sales-payment.model";

export class SalesInvoiceModel {
  /**
   * The unique identifier for the sales invoice.
   * @type {number}
   */
  invoiceId: number = 0;

  /**
   * Purchase date for this invoice.
   * @type {string}
   */
  salesDate: string = '';

  /**
   * The total sales price for the invoice.
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
   * The unique identifier for the organization associated with the sales invoice.
   * @type {number}
   */
  organizationId: number = 0;

  /**
   * The unique identifier for the sales invoice.
   * @type {string}
   */
  invoiceNumber: string = '';

  /**
   * The remarks associated with the sales invoice.
   * @type {string}
   */
  remarks: string = '';

  /**
   * The terms and conditions associated with the sales invoice.
   * @type {string}
   */
  terms: string = '';

  /**
   * Person or entity name of the customer
   * @type {string}
   */
  customerName: string = 'Guest User';

  /**
   * Person or entity details object of the customer
   * @type {string}
   */
  customer: CustomerModel | null = null;

  /**
   * An array of sales details associated with the sales invoice.
   * @type {Array<SalesInvoiceDetailsModel>}
   */
  salesDetails: Array<SalesInvoiceDetailsModel> = [];

  /**
   * Payment records associated with the sales invoice.
   * @type {Array<SalesPaymentModel>}
   */
  paymentRecords: Array<SalesPaymentModel> = [];
}
