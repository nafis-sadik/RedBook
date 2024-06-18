using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data.Entities;

public partial class SalesInvoice
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int InvoiceId { get; set; }

    public DateTime SalesDate { get; set; }

    public string SoldBy { get; set; }

    public decimal TotalAmount { get; set; }

    public int OrganizationId { get; set; }

    public string ChalanNo { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<SalesPaymentRecord> SalesPaymentRecords { get; set; } = new List<SalesPaymentRecord>();
}
