using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class RedBookInventoryContext : DbContext
    {
        public const string DefaultSchema = "Inventory";
        public RedBookInventoryContext(DbContextOptions<RedBookInventoryContext> options): base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CommonAttribute> CommonAttributes { get; set; }
        public DbSet<Inventory.Domain.Entities.Inventory> Inventory { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseDetails> PurchaseDetails { get; set; }
        public DbSet<PurchasePaymentRecord> PurchasePayments { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SalesDetails> SalesDetails { get; set; }
        public DbSet<SalesPaymentRecords> SalesPaymentRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var commonAttributes = new CommonAttribute[]
            {
                new CommonAttribute {
                    Id = 1,
                    AttributeName = "Liter",
                    AttributeType = "Quantity",
                    CreateDate = DateTime.UtcNow,
                    CreatedBy = null,
                    UpdateDate = null,
                    UpdatedBy = null
                },
                new CommonAttribute {
                    Id = 2,
                    AttributeName = "Kg",
                    AttributeType = "Quantity",
                    CreateDate = DateTime.UtcNow,
                    CreatedBy = null,
                    UpdateDate = null,
                    UpdatedBy = null
                },
                new CommonAttribute {
                    Id = 3,
                    AttributeName = "Meter",
                    AttributeType = "Quantity",
                    CreateDate = DateTime.UtcNow,
                    CreatedBy = null,
                    UpdateDate = null,
                    UpdatedBy = null
                }
            };
            modelBuilder.Entity<CommonAttribute>().HasData(commonAttributes);
        }
    }
}
