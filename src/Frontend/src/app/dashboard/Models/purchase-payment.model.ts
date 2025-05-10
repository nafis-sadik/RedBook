/**
 * Represents a purchase payment record, containing details about a payment made for a purchase invoice.
 */
export class PurchasePaymentModel {
  /**
   * Unique identifier of this payment record.
   */
  purchasePaymentId: number = 0;

  /**
   * The unique identifier for the purchase invoice that this payment record is associated with.
   */
  invoiceId: number = 0;

  /**
   * The unique identifier of the account associated with this payment.
   */
  accountId: number = 0;

  /**
   * The date the payment was made.
   */
  paymentDate: string = '';

  /**
   * The amount of the payment.
   */
  paymentAmount: number = 0;

  /**
   * The transaction reference for the payment.
   */
  transactionReference: string = '';

  /**
   * Any remarks or notes associated with the payment.
   */
  remarks: string = '';
}