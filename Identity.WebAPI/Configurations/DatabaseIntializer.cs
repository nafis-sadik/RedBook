﻿using Identity.Data.Entities;
using Microsoft.EntityFrameworkCore;
using RedBook.Core.Constants;

namespace Identity.WebAPI.Configurations
{
    public static class DatabaseIntializer
    {
        public static void InitDatabase(this IApplicationBuilder app, IWebHostEnvironment env)
        {

            using (var scope = app.ApplicationServices.CreateScope())
            {
                UserManagementSystemContext context = scope.ServiceProvider.GetRequiredService<UserManagementSystemContext>();
                                
                // Only run database migrations in development environment
                if (env.IsDevelopment())
                {
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                }
                
                // Eager load all resources
                context.Roles.Include(r => r.Policies).Include(r => r.Users).Load();
                context.Applications.Include(r => r.Routes).Load();
                context.OrganizationRoleMappings.Load();
                context.Organizations.Load();
                context.Users.Load();

                // Load Seed Data for Applications
                context.Applications.AddAsync(new Application {
                    Id = 1,
                    ApplicationName = "Redbook"
                });

                // Load Seed Data for Users
                context.Roles.AddAsync(new Role
                {
                    Id = 1,
                    IsGenericRole = false,
                    RoleName = "System Admin",
                });

                // Load Seed Data for Users
                context.Users.AddAsync(new User
                {
                    Id = new Guid().ToString(),
                    AccountBalance = int.MaxValue,
                    FirstName = "Md. Nafis",
                    LastName = "Sadik",
                    UserName = "nafis_sadik",
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword("SHARMIN<3nafis23"),
                    Status = CommonConstants.StatusTypes.Active.ToString(),
                    RoleId = 1,
                    OrganizationId = 1,
                });

                // Load Seed Data for Organizations
                context.Organizations.AddAsync(new Organization {
                    Id = 1,
                    OrganizationName = "Blume Digital Corp."
                });

                // Load Seed Data for Organization Role Mappings
                context.OrganizationRoleMappings.AddAsync(new OrganizationRoleMapping {
                    Id = 1,
                    OrganizationId = 1,
                    RoleId = 1,
                });

                context.SaveChangesAsync();
            }
        }
    }
}
