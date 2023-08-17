using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class OrganizationCache
{
    public int OrganizationId { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
