using Microsoft.EntityFrameworkCore;
using RedBook.Core.Constants;

namespace Identity.Data.Entities {
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("User Id=DESKTOP-SLFI9O5;Database=UserManagementSystem;Trusted_Connection=True;TrustServerCertificate=True;");

        //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    {
        //        if (!optionsBuilder.IsConfigured)
        //        {
        //            optionsBuilder.UseMySQL("server=containers-us-west-34.railway.app;uid=root;pwd=duAdo6cYZWNEXElxUKsf;port=7140;protocol=TCP;database=railway");
        //        }
        //    }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<OrganizationRoleMapping>(entity =>
            {
                entity.ToTable("OrganizationRoleMapping");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

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

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

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
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.HasIndex(e => e.ApplicationId, "IX_Routes_ApplicationId");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
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

                entity.Property(e => e.Id).HasMaxLength(50);
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.Password).IsRequired();
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
                    Id = CommonConstants.GenericRoles.SystemAdminRoleId,
                    RoleName = CommonConstants.GenericRoles.SystemAdmin,
                    IsGenericRole = true,
                },
                new Role
                {
                    Id = CommonConstants.GenericRoles.AdminRoleId,
                    RoleName = CommonConstants.GenericRoles.Admin,
                    IsGenericRole = true,
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
                    AccountBalance = int.MaxValue,
                    Status = CommonConstants.StatusTypes.Active,
                    OrganizationId = 1,
                    RoleId = 1,
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword("OBO13nafu.")
                }
            };
            modelBuilder.Entity<User>().HasData(SystemAdminUsers);


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}