using RedBook.Core.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Domain.Entities
{
    public class CommonAttribute: BaseEntity<int>
    {
        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string AttributeName { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string AttributeType { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
