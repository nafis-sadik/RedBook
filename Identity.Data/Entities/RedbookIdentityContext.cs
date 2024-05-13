using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data.Entities;

public partial class RedbookIdentityContext : DbContext
{
    public RedbookIdentityContext()
    {
    }

    public RedbookIdentityContext(DbContextOptions<RedbookIdentityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleRouteMapping> RoleRouteMappings { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<RouteType> RouteTypes { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<SubscriptionPackage> SubscriptionPackages { get; set; }

    public virtual DbSet<SubscriptionTransaction> SubscriptionTransactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRoleMapping> UserRoleMappings { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=RedbookIdentity;User ID=sa;TrustServerCertificate=True;Encrypt=False;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PK__Applicat__3214EC07D6504A6D");

            entity.Property(e => e.ApplicationName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApplicationUrl)
                .IsRequired()
                .HasDefaultValueSql("(N'')");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.LogoUrl).HasMaxLength(100);
            entity.Property(e => e.OrganizationName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.OrganizationId, "IX_Roles_OrganizationId");

            entity.Property(e => e.IsAdmin)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.IsRetailer)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.IsSystemAdmin)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Application).WithMany(p => p.Roles)
                .HasForeignKey(d => d.ApplicationId)
                .HasConstraintName("FK_Roles_Applications");

            entity.HasOne(d => d.Organization).WithMany(p => p.Roles)
                .HasForeignKey(d => d.OrganizationId)
                .HasConstraintName("FK_Roles_Organizations");
        });

        modelBuilder.Entity<RoleRouteMapping>(entity =>
        {
            entity.HasKey(e => e.MappingId);

            entity.ToTable("RoleRouteMapping");

            entity.HasIndex(e => e.RoleId, "IX_RoleRouteMapping_RoleId");

            entity.HasIndex(e => e.RouteId, "IX_RoleRouteMapping_RouteId");

            entity.HasOne(d => d.Role).WithMany(p => p.RoleRouteMappings)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleRouteMapping_Roles");

            entity.HasOne(d => d.Route).WithMany(p => p.RoleRouteMappings)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleRouteMapping_Routes");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasIndex(e => e.ApplicationId, "IX_Routes_ApplicationId");

            entity.HasIndex(e => e.ParentRouteId, "IX_Routes_ParentRouteId");

            entity.HasIndex(e => e.RouteTypeId, "IX_Routes_RouteTypeId");

            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Route1)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Route");
            entity.Property(e => e.RouteName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Application).WithMany(p => p.Routes)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Routes_Applications");

            entity.HasOne(d => d.ParentRoute).WithMany(p => p.InverseParentRoute).HasForeignKey(d => d.ParentRouteId);

            entity.HasOne(d => d.RouteType).WithMany(p => p.Routes)
                .HasForeignKey(d => d.RouteTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Routes_RouteTypes");
        });

        modelBuilder.Entity<RouteType>(entity =>
        {
            entity.Property(e => e.RouteTypeName).IsRequired();
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.ToTable("Subscription");

            entity.Property(e => e.SubscriptionId).ValueGeneratedNever();
            entity.Property(e => e.CurrentExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.SubscriptionFee).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SubscriptionStartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Organization).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subscription_Organizations");

            entity.HasOne(d => d.Package).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subscription_SubscriptionPackages");
        });

        modelBuilder.Entity<SubscriptionPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId);

            entity.Property(e => e.PackageId).ValueGeneratedNever();
            entity.Property(e => e.PackageName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.SubscriptionFee).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Application).WithMany(p => p.SubscriptionPackages)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubscriptionPackages_Applications");
        });

        modelBuilder.Entity<SubscriptionTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId);

            entity.Property(e => e.TransactionId).ValueGeneratedNever();
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");

            entity.HasOne(d => d.PaidByNavigation).WithMany(p => p.SubscriptionTransactions)
                .HasForeignKey(d => d.PaidBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubscriptionTransactions_Users");

            entity.HasOne(d => d.Subscription).WithMany(p => p.SubscriptionTransactions)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubscriptionTransactions_Subscription");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Email)
                .IsRequired()
                .HasDefaultValueSql("(N'')");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserRoleMapping>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK_UserRoles");

            entity.ToTable("UserRoleMapping");

            entity.HasIndex(e => e.RoleId, "IX_UserRoles_RoleId");

            entity.HasIndex(e => e.UserId, "IX_UserRoles_UserId");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoleMappings)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_UserRoles_Roles_RoleId");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoleMappings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserRoles_Users_UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
