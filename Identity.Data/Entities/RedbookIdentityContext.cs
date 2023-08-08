﻿using Microsoft.EntityFrameworkCore;

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

    public virtual DbSet<OrganizationRoleMapping> OrganizationRoleMappings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleRouteMapping> RoleRouteMappings { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-SLFI9O5;User Id=sa;Database=RedbookIdentity;TrustServerCertificate=True;Encrypt=False;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PK__Applicat__3214EC07D6504A6D");

            entity.Property(e => e.ApplicationId).ValueGeneratedNever();
            entity.Property(e => e.ApplicationName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.Property(e => e.OrganizationId).ValueGeneratedNever();
            entity.Property(e => e.OrganizationName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OrganizationRoleMapping>(entity =>
        {
            entity.HasKey(e => e.MappingId);

            entity.ToTable("OrganizationRoleMapping");

            entity.Property(e => e.MappingId).ValueGeneratedNever();

            entity.HasOne(d => d.Organization).WithMany(p => p.OrganizationRoleMappings)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrganizationRoleMapping_Organizations");

            entity.HasOne(d => d.Role).WithMany(p => p.OrganizationRoleMappings)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrganizationRoleMapping_Roles");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RoleRouteMapping>(entity =>
        {
            entity.HasKey(e => e.MappingId);

            entity.ToTable("RoleRouteMapping");

            entity.Property(e => e.MappingId).ValueGeneratedNever();

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
            entity.Property(e => e.RouteId).ValueGeneratedNever();
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

            entity.HasOne(d => d.Organization).WithMany(p => p.Users)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Organizations");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
