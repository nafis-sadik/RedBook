using Identity.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

namespace Identity.Data
{
    public partial class UserManagementSystemContext : DbContext
    {
        public UserManagementSystemContext() { }

        public UserManagementSystemContext(DbContextOptions<UserManagementSystemContext> options) : base(options) { }

        public const string DefaultSchema = "UserManagement";
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Policy> Policies { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("server=containers-us-west-34.railway.app;uid=root;pwd=duAdo6cYZWNEXElxUKsf;port=7140;protocol=TCP;database=railway");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Application>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ApplicationName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.OrganizationName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Policy>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Policies)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Policies_Roles");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.Policies)
                    .HasForeignKey(d => d.RouteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Policies_Routes");

                entity.HasOne(d => d.UserGroup)
                    .WithMany(p => p.Policies)
                    .HasForeignKey(d => d.UserGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Policies_UserGroups");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Roles_Organizations");
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Route1)
                    .IsRequired()
                    .HasColumnName("Route");

                entity.Property(e => e.RouteName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.Routes)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Routes_Applications");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_Users_Organizations");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Users_Roles");
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.UserGroupName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.UserGroups)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserGroups_Organizations");
            });

            var DefaultOrgs = new Organization[]
            {
                new Organization
                {
                    Id = 1,
                    OrganizationName = "Blume Digital Corp.",
                }
            };
            modelBuilder.Entity<Organization>().HasData(DefaultOrgs);

            var SysAdminRole = new Role[] {
                new Role
                {
                    Id = 1,
                    RoleName = "System Admin",
                    OrganizationId = 1,
                }
            };
            modelBuilder.Entity<Role>().HasData(SysAdminRole);

            var SystemAdminUsers = new User[]
            {
                new User {
                    Id = new Guid().ToString(),
                    FirstName = "Nafis",
                    LastName = "Sadik",
                    UserName = "nafis_sadik",
                    AccountBalance = 999999999999,
                    Status = 'A',
                    OrganizationId = 1,
                    RoleId = 1,
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword("OBO13nafu.")
                }
            };
            modelBuilder.Entity<User>().HasData(SystemAdminUsers);

            //OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
