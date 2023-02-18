using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public partial class RedBookInventoryContext : DbContext
    {
        public RedBookInventoryContext() { }

        public RedBookInventoryContext(DbContextOptions<RedBookInventoryContext> options) : base(options) { }

        public const string DefaultSchema = "Inventory";
        public virtual DbSet<Catagory> Categories { get; set; }
        public virtual DbSet<CommonAttribute> CommonAttributes { get; set; }
        public virtual DbSet<Inventory.Domain.Entities.Inventory> Inventory { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual DbSet<PurchasePaymentRecord> PurchasePayments { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SalesDetail> SalesDetails { get; set; }
        public virtual DbSet<SalesPaymentRecord> SalesPaymentRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BankName).IsRequired();
            });

            modelBuilder.Entity<BankBranch>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BranchName).IsRequired();

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.BankBranches)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BankBranches_Banks");
            });

            modelBuilder.Entity<Catagory>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CatagoryName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ParentCategoryNavigation)
                    .WithMany(p => p.InverseParentCategoryNavigation)
                    .HasForeignKey(d => d.ParentCategory)
                    .HasConstraintName("FK_Catagories_Catagories");
            });

            modelBuilder.Entity<CommonAttribute>(entity =>
            {
                entity.ToTable("CommonAttribute");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AttributeName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AttributeType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Inventory.Domain.Entities.Inventory>(entity =>
            {
                entity.ToTable("Inventory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ChalanNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.ChalanNumberNavigation)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.ChalanNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_Purchase");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_Products");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ProductName).IsRequired();

                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UpdateBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.ToTable("Purchase");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CheckNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PurchaseDate).HasColumnType("datetime");

                entity.Property(e => e.PurchasedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TotalPurchasePrice).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.FromBankBranch)
                    .WithMany(p => p.PurchaseFromBankBranches)
                    .HasForeignKey(d => d.FromBankBranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchase_BankBranches1");

                entity.HasOne(d => d.FromBank)
                    .WithMany(p => p.PurchaseFromBanks)
                    .HasForeignKey(d => d.FromBankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchase_Banks1");

                entity.HasOne(d => d.ToBankBranch)
                    .WithMany(p => p.PurchaseToBankBranches)
                    .HasForeignKey(d => d.ToBankBranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchase_BankBranches");

                entity.HasOne(d => d.ToBank)
                    .WithMany(p => p.PurchaseToBanks)
                    .HasForeignKey(d => d.ToBankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchase_Banks");
            });

            modelBuilder.Entity<PurchaseDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ChalanNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.ChalanNumberNavigation)
                    .WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.ChalanNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseDetails_Purchase1");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseDetails_Products");
            });

            modelBuilder.Entity<PurchasePaymentRecord>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ChalanNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.HasOne(d => d.ChalanNumberNavigation)
                    .WithMany(p => p.PurchasePaymentRecords)
                    .HasForeignKey(d => d.ChalanNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchasePaymentRecords_Purchase");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.SalesDate).HasColumnType("datetime");

                entity.Property(e => e.SoldBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<SalesDetail>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ChalanNo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MemoNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.ChalanNoNavigation)
                    .WithMany(p => p.SalesDetails)
                    .HasForeignKey(d => d.ChalanNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesDetails_Purchase");

                entity.HasOne(d => d.MemoNumberNavigation)
                    .WithMany(p => p.SalesDetails)
                    .HasForeignKey(d => d.MemoNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesDetails_Sales");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SalesDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesDetails_Products");
            });

            modelBuilder.Entity<SalesPaymentRecord>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.MemoNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.HasOne(d => d.MemoNumberNavigation)
                    .WithMany(p => p.SalesPaymentRecords)
                    .HasForeignKey(d => d.MemoNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesPaymentRecords_Sales");
            });

            var commonAttributes = new CommonAttribute[]
            {
                new CommonAttribute {
                    Id = 1,
                    AttributeName = "Liter",
                    AttributeType = "Quantity",
                    CreateDate = DateTime.UtcNow,
                    CreatedBy = "SystemAdmin"
                },
                new CommonAttribute {
                    Id = 2,
                    AttributeName = "Kg",
                    AttributeType = "Quantity",
                    CreateDate = DateTime.UtcNow,
                    CreatedBy = "SystemAdmin"
                },
                new CommonAttribute {
                    Id = 3,
                    AttributeName = "Meter",
                    AttributeType = "Quantity",
                    CreateDate = DateTime.UtcNow,
                    CreatedBy = "SystemAdmin"
                }
            };
            modelBuilder.Entity<CommonAttribute>().HasData(commonAttributes);
        }
    }
}
