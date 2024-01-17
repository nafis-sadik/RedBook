namespace Inventory.Data.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        public string CatagoryName { get; set; }

        public int? ParentCategory { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
