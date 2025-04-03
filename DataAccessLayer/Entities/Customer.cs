using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Inventory.Data.Entities
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [AllowNull]
        public string Email { get; set; }

        public virtual ICollection<CustomerDetails> CustomerDetails { get; set; } = new List<CustomerDetails>();
    }
}
