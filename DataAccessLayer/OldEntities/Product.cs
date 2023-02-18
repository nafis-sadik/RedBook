using RedBook.Core.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Domain.Entities
{
    public class Product: BaseEntity<int>
    {
        [MaxLength(200)]
        [Column(TypeName = "varchar(200)")]
        public string? ProductName { get; set; }
        public decimal Quantity { get; set; }
        [Required]
        public int QuantityAttributeId { get; set; }
        public virtual CommonAttribute QuantityAttribute { get; set; } = new CommonAttribute();
        public DateTime CreateDate { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid UpdateBy { get; set; }
    }
}