using Microsoft.EntityFrameworkCore;

namespace Inventory.Data.Entities;

public partial class RedbookInventoryContext : DbContext
{
    public RedbookInventoryContext()
    {
    }

    public RedbookInventoryContext(DbContextOptions<RedbookInventoryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<BankBranch> BankBranches { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CommonAttribute> CommonAttributes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<PurchaseInvoiceDetails> PurchaseRecords { get; set; }

    public virtual DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }

    public virtual DbSet<PurchasePaymentRecord> PurchasePaymentRecords { get; set; }

    public virtual DbSet<SalesInvoice> SalesInvoice { get; set; }

    public virtual DbSet<SalesInvoiceDetails> SalesRecords { get; set; }

    public virtual DbSet<SalesPaymentRecord> SalesPaymentRecords { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=RedbookInventory;User ID=sa;TrustServerCertificate=True;Encrypt=False;Trusted_Connection=True;");
}
