using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CatagoryName { get; set; }

    public int? ParentCategory { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual UserCache CreatedByNavigation { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual UserCache UpdatedByNavigation { get; set; }
}
