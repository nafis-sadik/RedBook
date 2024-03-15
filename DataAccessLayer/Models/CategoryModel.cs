namespace Inventory.Data.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        public string CatagoryName { get; set; }

        public int? ParentCategoryId { get; set; }

        public int OrganizationId { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
