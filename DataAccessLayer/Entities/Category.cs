namespace Inventory.Data.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CatagoryName { get; set; }

    public int? ParentCategory { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? UpdatedBy { get; set; }

    public int OrganizationId { get; set; }

    //public virtual UserCache CreatedByNavigation { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    //public virtual UserCache UpdatedByNavigation { get; set; }
}
