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

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }

    public virtual DbSet<PurchasePaymentRecord> PurchasePaymentRecords { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SalesInvoice> SalesInvoices { get; set; }

    public virtual DbSet<SalesPaymentRecord> SalesPaymentRecords { get; set; }

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

            entity.HasIndex(e => e.BranchId, "IX_BankAccounts_BranchId");

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

            entity.HasIndex(e => e.BankId, "IX_BankBranch_BankId");

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
            entity.HasIndex(e => e.ParentCategoryId, "IX_Categories_ParentCategoryId");

            entity.Property(e => e.CatagoryName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasPrecision(0);
            entity.Property(e => e.UpdateDate).HasPrecision(0);
            entity.Property(e => e.ParentCategoryId).IsRequired(false);
            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory).HasForeignKey(d => d.ParentCategoryId);
        });

        modelBuilder.Entity<CommonAttribute>(entity =>
        {
            entity.HasKey(e => e.AttributeId);

            entity.ToTable("CommonAttribute");

            entity.HasIndex(e => e.AttributeId, "PK__CommonAt__3214EC0712D1E7C6").IsUnique();

            entity.Property(e => e.AttributeName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AttributeType)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasPrecision(0);
            entity.Property(e => e.UpdateDate).HasPrecision(0);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.CategoryId, "IX_Products_CategoryId");

            entity.HasIndex(e => e.QuantityAttributeId, "IX_Products_QuantityAttributeId");

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

            entity.HasOne(d => d.QuantityAttribute).WithMany(p => p.Products)
                .HasForeignKey(d => d.QuantityAttributeId)
                .HasConstraintName("FK_Products_CommonAttribute");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PK_PurchaseDetails");

            entity.ToTable("Purchase");

            entity.HasIndex(e => e.ProductId, "IX_PurchaseDetails_ProductId");

            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchase_PurchaseInvoice");

            entity.HasOne(d => d.Product).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseDetails_Products");
        });

        modelBuilder.Entity<PurchaseInvoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK_Purchase");

            entity.ToTable("PurchaseInvoice");

            entity.Property(e => e.ChalanNumber)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CheckNumber)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasPrecision(0);
            entity.Property(e => e.PurchaseDate).HasPrecision(0);
            entity.Property(e => e.TotalPurchasePrice).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<PurchasePaymentRecord>(entity =>
        {
            entity.HasKey(e => e.PurchasePaymentId).HasName("PK_PurchasePayments");

            entity.HasIndex(e => e.InvoiceId, "IX_PurchasePayments_PurchaseId");

            entity.Property(e => e.PurchasePaymentId).ValueGeneratedNever();
            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PaymentDate).HasPrecision(0);

            entity.HasOne(d => d.Invoice).WithMany(p => p.PurchasePaymentRecords)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePayments_Purchase");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SalesId).HasName("PK_SalesDetails");

            entity.HasIndex(e => e.ProductId, "IX_SalesDetails_ProductId");

            entity.HasIndex(e => e.InvoiceId, "IX_SalesDetails_SalesId");

            entity.Property(e => e.ChalanNo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasPrecision(0);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Sales)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesDetails_Sales");

            entity.HasOne(d => d.Product).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesDetails_Products");
        });

        modelBuilder.Entity<SalesInvoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK_Sales");

            entity.ToTable("SalesInvoice");

            entity.Property(e => e.SalesDate).HasPrecision(0);
            entity.Property(e => e.SoldBy)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<SalesPaymentRecord>(entity =>
        {
            entity.HasKey(e => e.SalesPaymentId);

            entity.HasIndex(e => e.InvoiceId, "IX_SalesPaymentRecords_SalesId");

            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PaymentDate).HasPrecision(0);

            entity.HasOne(d => d.Invoice).WithMany(p => p.SalesPaymentRecords)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesPaymentRecords_Sales");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
