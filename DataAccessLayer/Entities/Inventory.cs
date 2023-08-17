namespace Inventory.Data.Entities;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int ProductId { get; set; }

    public decimal Quantity { get; set; }

    public int PurchaseId { get; set; }

    public int QuantityAttributeId { get; set; }

    public int OrganizationId { get; set; }

    public virtual OrganizationCache Organization { get; set; }

    public virtual Purchase Purchase { get; set; }
}
