using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class CommonAttribute
{
    public string AttributeType { get; set; }

    public string AttributeName { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? UpdateBy { get; set; }

    public int AttributeId { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
