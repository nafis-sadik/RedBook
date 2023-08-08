using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data.Entities;

public partial class UserManagementSystemContext : DbContext
{
    public UserManagementSystemContext()
    {
    }

    public UserManagementSystemContext(DbContextOptions<UserManagementSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<OrganizationRoleMapping> OrganizationRoleMappings { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySQL("server=localhost;User Id=root;Port=3306;protocol=TCP;database=RedbookIdentity;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.Property(e => e.ApplicationName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.Property(e => e.OrganizationName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<OrganizationRoleMapping>(entity =>
        {
            entity.ToTable("OrganizationRoleMapping");

            entity.HasIndex(e => e.OrganizationId, "IX_OrganizationRoleMapping_OrganizationId");

            entity.HasIndex(e => e.RoleId, "IX_OrganizationRoleMapping_RoleId");

            entity.HasOne(d => d.Organization).WithMany(p => p.OrganizationRoleMappings)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrganizationRoleMapping_Organizations");

            entity.HasOne(d => d.Role).WithMany(p => p.OrganizationRoleMappings)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrganizationRoleMapping_Roles");
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_Policies_RoleId");

            entity.HasIndex(e => e.RouteId, "IX_Policies_RouteId");

            entity.HasIndex(e => e.UserGroupId, "IX_Policies_UserGroupId");

            entity.HasOne(d => d.Role).WithMany(p => p.Policies)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policies_Roles");

            entity.HasOne(d => d.Route).WithMany(p => p.Policies)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policies_Routes");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasIndex(e => e.ApplicationId, "IX_Routes_ApplicationId");

            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Route1)
                .IsRequired()
                .HasColumnName("Route");
            entity.Property(e => e.RouteName)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Application).WithMany(p => p.Routes)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Routes_Applications");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.OrganizationId, "IX_Users_OrganizationId");

            entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(1);
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.Organization).WithMany(p => p.Users)
                .HasForeignKey(d => d.OrganizationId)
                .HasConstraintName("FK_Users_Organizations");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
