﻿using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public decimal Quantity { get; set; }

    public int QuantityAttributeId { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? UpdateBy { get; set; }

    public int OrganizationId { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual CommonAttribute QuantityAttribute { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
