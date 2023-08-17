using System;
using System.Collections.Generic;
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

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<OrganizationCache> OrganizationCaches { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }

    public virtual DbSet<PurchasePayment> PurchasePayments { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SalesDetail> SalesDetails { get; set; }

    public virtual DbSet<SalesPaymentRecord> SalesPaymentRecords { get; set; }

    public virtual DbSet<UserCache> UserCaches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=RedbookInventory;User ID=sa;TrustServerCertificate=True;Encrypt=False;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.BankId).HasName("PK__Bank__3214EC07A30FEA48");

            entity.ToTable("Bank");

            entity.Property(e => e.BankName)
                .IsRequired()
                .IsUnicode(false);
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.Property(e => e.AccountName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Branch).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankAccounts_BankBranch");
        });

        modelBuilder.Entity<BankBranch>(entity =>
        {
            entity.HasKey(e => e.BranchId);

            entity.ToTable("BankBranch");

            entity.Property(e => e.BranchName)
                .IsRequired()
                .IsUnicode(false);

            entity.HasOne(d => d.Bank).WithMany(p => p.BankBranches)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankBranches_Banks");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CatagoryName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasPrecision(0);
            entity.Property(e => e.UpdateDate).HasPrecision(0);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Categories_UserCache");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CategoryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_Categories_UserCache1");
        });

        modelBuilder.Entity<CommonAttribute>(entity =>
        {
            entity.HasKey(e => e.AttributeId).HasName("PK__CommonAt__3214EC0712D1E7C6");

            entity.ToTable("CommonAttribute");

            entity.Property(e => e.AttributeId).ValueGeneratedNever();
            entity.Property(e => e.AttributeName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AttributeType)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasPrecision(0);
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasPrecision(0);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.ToTable("Inventory");

            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Organization).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_OrganizationCache");

            entity.HasOne(d => d.Purchase).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Purchase");
        });

        modelBuilder.Entity<OrganizationCache>(entity =>
        {
            entity.HasKey(e => e.OrganizationId);

            entity.ToTable("OrganizationCache");

            entity.Property(e => e.OrganizationId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.CreateDate).HasPrecision(0);
            entity.Property(e => e.ProductName)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UpdateDate).HasPrecision(0);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Categories");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.ProductCreateByNavigations)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_UserCache1");

            entity.HasOne(d => d.Organization).WithMany(p => p.Products)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_OrganizationCache");

            entity.HasOne(d => d.QuantityAttribute).WithMany(p => p.Products)
                .HasForeignKey(d => d.QuantityAttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_CommonAttribute");

            entity.HasOne(d => d.UpdateByNavigation).WithMany(p => p.ProductUpdateByNavigations)
                .HasForeignKey(d => d.UpdateBy)
                .HasConstraintName("FK_Products_UserCache");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.ToTable("Purchase");

            entity.Property(e => e.ChalanNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CheckNumber)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PurchaseDate).HasPrecision(0);
            entity.Property(e => e.TotalPurchasePrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Account).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchase_BankAccounts");
        });

        modelBuilder.Entity<PurchaseDetail>(entity =>
        {
            entity.HasKey(e => e.PurchaseDetailsId);

            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseDetails_Products");

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseDetails_Purchase");
        });

        modelBuilder.Entity<PurchasePayment>(entity =>
        {
            entity.Property(e => e.PurchasePaymentId).ValueGeneratedNever();
            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PaymentDate).HasPrecision(0);

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchasePayments)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePayments_Purchase");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SalesId);

            entity.Property(e => e.SalesDate).HasPrecision(0);
            entity.Property(e => e.SoldBy)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<SalesDetail>(entity =>
        {
            entity.HasKey(e => e.SalesDetailsId);

            entity.Property(e => e.ChalanNo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Product).WithMany(p => p.SalesDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesDetails_Products");

            entity.HasOne(d => d.Sales).WithMany(p => p.SalesDetails)
                .HasForeignKey(d => d.SalesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesDetails_Sales");

            entity.HasOne(d => d.SoldByNavigation).WithMany(p => p.SalesDetails)
                .HasForeignKey(d => d.SoldBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesDetails_UserCache");
        });

        modelBuilder.Entity<SalesPaymentRecord>(entity =>
        {
            entity.HasKey(e => e.SalesPaymentId);

            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PaymentDate).HasPrecision(0);

            entity.HasOne(d => d.Sales).WithMany(p => p.SalesPaymentRecords)
                .HasForeignKey(d => d.SalesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesPaymentRecords_Sales");
        });

        modelBuilder.Entity<UserCache>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("UserCache");

            entity.Property(e => e.UserId).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
