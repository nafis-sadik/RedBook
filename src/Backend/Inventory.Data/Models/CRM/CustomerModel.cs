using System.ComponentModel.DataAnnotations;

namespace Inventory.Data.Models.CRM
{
    public class CustomerModel
    {
        [Required]
        public int CustomerId { get; set; }

        public int OrgId { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        public string ContactNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Remarks { get; set; } = string.Empty;
    }
}
