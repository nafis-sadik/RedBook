namespace Inventory.Data.Models.Purchase
{
    public class InvoiceDetailsModel
    {
        public int RecordId { get; set; }

        public int InvoiceId { get; set; }

        public int ProductId { get; set; }

        public int ProductName { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public int? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
