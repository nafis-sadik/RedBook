using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RedBook.Core.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Domain.Entities
{
    public class Bank : BaseEntity<int>
    {
        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string BankName { get; set; }

        public virtual ICollection<BankBranches> BankBranches { get; set; } = new List<BankBranches>();
    }
}
