namespace Inventory.Data.Models.Purchase
{
    public class PaymentModel
    {
        public int PurchasePaymentId { get; set; }

        public int InvoiceId { get; set; }

        public int? AccountId { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal PaymentAmount { get; set; }

        public string TransactionReference { get; set; }

        public string Remarks { get; set; }
    }
}
