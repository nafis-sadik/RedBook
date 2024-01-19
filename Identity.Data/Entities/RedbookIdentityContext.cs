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

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<RouteType> RouteTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PK__Applicat__3214EC07D6504A6D");

            entity.HasIndex(e => e.OrganizationId, "IX_Applications_OrganizationId");

            entity.Property(e => e.ApplicationName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Organization).WithMany(p => p.Applications)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Applications_Organizations");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.Property(e => e.OrganizationName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.OrganizationId, "IX_Roles_OrganizationId");

            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Organization).WithMany(p => p.Roles)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
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

            entity.Property(e => e.Description)
                .IsRequired()
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
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
