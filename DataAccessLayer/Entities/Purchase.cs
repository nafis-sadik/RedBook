namespace Inventory.Data.Entities;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public int PurchasedBy { get; set; }

    public int AccountId { get; set; }

    public string CheckNumber { get; set; }

    public decimal TotalPurchasePrice { get; set; }

    public int OrganizationId { get; set; }

    public string ChalanNumber { get; set; }

    public virtual BankAccount Account { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<PurchasePayment> PurchasePayments { get; set; } = new List<PurchasePayment>();
}
