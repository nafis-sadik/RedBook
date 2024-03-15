using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CatagoryName { get; set; }

    public int? ParentCategoryId { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? UpdatedBy { get; set; }

    public int OrganizationId { get; set; }

    public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>();

    public virtual Category ParentCategory { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
