namespace Inventory.Data.Models
{
    public class SalesInvoiceModel
    {
        public int InvoiceId { get; set; }

        public DateTime SalesDate { get; set; }

        public string SoldBy { get; set; }

        public decimal TotalAmount { get; set; }

        public int OrganizationId { get; set; }
    }
}
