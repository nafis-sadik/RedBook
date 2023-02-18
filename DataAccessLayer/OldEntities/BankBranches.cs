using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RedBook.Core.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Domain.Entities
{
    public class BankBranches : BaseEntity<int>
    {
        [MaxLength(200)]
        [Column(TypeName = "varchar(200)")]
        public string? BranchName { get; set; }
        //public virtual Bank Bank { get; set; } = new Bank();
    }
}
