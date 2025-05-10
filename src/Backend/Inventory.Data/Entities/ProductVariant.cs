using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data.Entities
{
    public partial class ProductVariant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VariantId { get; set; }

        public string VariantName { get; set; }

        public string SKU { get; set; }

        public decimal StockQuantity { get; set; }

        public string BarCode { get; set; }

        public string Attributes { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateBy { get; set; }

        public int ProductId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public virtual ICollection<PurchaseInvoiceDetails> Purchases { get; set; } = new List<PurchaseInvoiceDetails>();
    }
}
