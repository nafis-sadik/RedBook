export declare interface IPaymentModel {
    /**
     * Payment Id
     * @type {number}
     */
    id: number;
    /**
     * Payment Date
     * @type {Date | string}
     */
    PaymentDate: Date | string;
    /**
     * Payment Amount
     * @type {number}
     */
    PaymentAmount: number;
    /**
     * Id of Invoice, this payment was recorded against
     * @type {number}
     */
    InvoiceId: number;
    /**
     * Total amount of bill of this Invoice
     * @type {number}
     */
    InvoiceTotalAmount: number;
    /**
     * Total due amount after this payment 
     * @type {number}
     */
    TotalDueAmount: number;
}