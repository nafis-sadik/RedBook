using Inventory.Data.Models.CRM;

namespace Inventory.Data.Models.Sales
{
    public class SalesInvoiceModel
    {
        /// <summary>
        /// The unique identifier for the sales invoice.
        /// </summary>
        public int InvoiceId { get; set; } = 0;

        /// <summary>
        /// Purchase date for this invoice.
        /// </summary>
        public string SalesDate { get; set; } = string.Empty;

        /// <summary>
        /// The total sales price for the invoice.
        /// </summary>
        public decimal InvoiceTotal { get; set; } = 0;

        /// <summary>
        /// The total amount of money paid to this vendor against this invoice.
        /// </summary>
        public decimal TotalPaid { get; set; } = 0;

        /// <summary>
        /// The total discount value on the invoice.
        /// </summary>
        public decimal TotalDiscount { get; set; } = 0;

        /// <summary>
        /// Current status of payment for this invoice
        /// </summary>
        public string PaymentStatus { get; set; } = RedBook.Core.Constants.CommonConstants.PaymentStatus.PartiallyPaid;

        /// <summary>
        /// The unique identifier for the organization associated with the sales invoice.
        /// </summary>
        public int OrganizationId { get; set; } = 0;

        /// <summary>
        /// The unique identifier for the sales invoice.
        /// </summary>
        public string InvoiceNumber { get; set; } = string.Empty;

        /// <summary>
        /// The remarks associated with the sales invoice.
        /// </summary>
        public string Remarks { get; set; } = string.Empty;

        /// <summary>
        /// The terms and conditions associated with the sales invoice.
        /// </summary>
        public string Terms { get; set; } = string.Empty;

        /// <summary>
        /// Person or entity, you are purchasing from.
        /// </summary>
        public CustomerModel? Customer { get; set; } = null;

        /// <summary>
        /// An array of sales details associated with the sales invoice.
        /// </summary>
        public List<SalesInvoiceDetailsModel> SalesDetails { get; set; } = new List<SalesInvoiceDetailsModel>();

        /// <summary>
        /// Payment records associated with the sales invoice.
        /// </summary>
        public List<SalesPaymentModel> PaymentRecords { get; set; } = new List<SalesPaymentModel>();
    }
}
