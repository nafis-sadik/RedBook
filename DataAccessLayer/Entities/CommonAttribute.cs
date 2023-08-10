using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class CommonAttribute
{
    public int AttributeId { get; set; }

    public string AttributeType { get; set; }

    public string AttributeName { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string UpdateBy { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
