using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class UserCache
{
    public int UserId { get; set; }

    public virtual ICollection<Category> CategoryCreatedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<Category> CategoryUpdatedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<Product> ProductCreateByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductUpdateByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();
}
