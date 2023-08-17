using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities;

public partial class SalesDetail
{
    public int SalesDetailsId { get; set; }

    public int SalesId { get; set; }

    public int ProductId { get; set; }

    public string ChalanNo { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int SoldBy { get; set; }

    public virtual Product Product { get; set; }

    public virtual Sale Sales { get; set; }

    public virtual UserCache SoldByNavigation { get; set; }
}
