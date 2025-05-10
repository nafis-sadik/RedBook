export class InvoicePaymentModel{
  /**
   * Primary key of payment records
   * @type {Number}
   */
  purchasePaymentId: number;
  /**
   * Invoice id of this payment
   * @type {Number}
   */
  invoiceId: number;
  /**
   * Payment amount of this payment record
   * @type {Number}
   */
  accountId: number;
  /**
   * Payment date of this payment record
   * @type {string}
   */
  paymentDate: string;
  /**
   * Total bill amount of this invoice
   * @type {Number}
   */
  paymentAmount: number;
  /**
   * Some short message to reference this payment record
   * @type {string}
   */
  transactionReference: string;
  /**
   * Some short message to describe this payment record
   * @type {string}
   */
  remarks: string;
  /**
   * Total invoice bill amount
   * @type {string}
   */
  invoiceTotalAmount: number;
  /**
   * Total due invoice bill amount
   * @type {string}
   */
  totalDueAmount: number;
}
