using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data.Entities
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string ContactNumber { get; set; }

        public string Address { get; set; }

        public string Remarks { get; set; }

        public virtual ICollection<SalesPaymentRecord> SalesPaymentRecords { get; set; } = new List<SalesPaymentRecord>();
    }
}
