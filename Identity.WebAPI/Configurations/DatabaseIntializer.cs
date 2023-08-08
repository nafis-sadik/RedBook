using Identity.Data.Entities;
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
                RedbookIdentityContext context = scope.ServiceProvider.GetRequiredService<RedbookIdentityContext>();
                                
                // Only run database migrations in development environment
                if (env.IsDevelopment())
                {
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();

                        // Eager load all resources
                        context.Applications.Include(r => r.Routes).Load();
                        context.OrganizationRoleMappings.Load();
                        context.Organizations.Load();
                        context.Users.Load();

                        // Load Seed Data for Applications
                        context.Applications.AddAsync(new Application
                        {
                            ApplicationId = 1,
                            ApplicationName = "Redbook"
                        });

                        // Load Seed Data for Routes
                        context.Routes.AddAsync(new Data.Entities.Route
                        {
                            RouteId = 1,
                            RouteName = "Redbook",
                            Route1 = "",
                            Description = "",
                            ApplicationId = 1
                        });

                        // Load Seed Data for Role-Route Mapping
                        context.RoleRouteMappings.AddAsync(new RoleRouteMapping
                        {
                            MappingId = 1,
                            RoleId = 1,
                            RouteId = 1
                        });

                        // Load Seed Data for Users
                        context.Roles.AddAsync(new Role
                        {
                            RoleId = 1,
                            IsGenericRole = 1,
                            RoleName = "System Admin",
                        });

                        // Load Seed Data for Organization-Role Mapping
                        context.OrganizationRoleMappings.AddAsync(new OrganizationRoleMapping { 
                            MappingId = 1,
                            RoleId= 1,
                            OrganizationId = 1
                        });

                        // Load Seed Data for Organizations
                        context.Organizations.AddAsync(new Organization
                        {
                            OrganizationId = 1,
                            OrganizationName = "Blume Digital Corp."
                        });

                        // Load Seed Data for Users
                        context.Users.AddAsync(new User
                        {
                            UserId = new Guid().ToString(),
                            AccountBalance = int.MaxValue,
                            FirstName = "Md. Nafis",
                            LastName = "Sadik",
                            UserName = "nafis_sadik",
                            Password = BCrypt.Net.BCrypt.EnhancedHashPassword("SHARMIN<3nafis23"),
                            Status = CommonConstants.StatusTypes.Active.ToString(),
                            RoleId = 1,
                            OrganizationId = 1,
                        });

                        context.SaveChanges();

                        context.Dispose();
                    }
                }
            }
        }
    }
}
