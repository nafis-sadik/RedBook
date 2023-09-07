using Identity.Data.Entities;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using RedBook.Core.Constants;

namespace Identity.WebAPI.Configurations
{
    public static class DatabaseIntializer
    {
        public static async void InitDatabase(this IApplicationBuilder app, IWebHostEnvironment env)
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
                    }

                    // Load Seed Data for Organizations
                    var org = await context.Organizations.FindAsync(1);
                    if (org == null)
                        await context.Organizations.AddAsync(new Organization
                        {
                            OrganizationName = "Blume Digital Corp.",
                            CreateDate = DateTime.Now,
                        });
                    else
                    {
                        org.OrganizationName = "Blume Digital Corp.";
                        org.CreateDate = DateTime.Now;
                    }

                    await context.SaveChangesAsync();

                    // Load Seed Data for Applications
                    var application = await context.Applications.FindAsync(1);
                    if (application == null)
                    {
                        await context.Applications.AddAsync(new Application
                        {
                            ApplicationName = "Redbook",
                            OrganizationId = 1
                        });
                    }
                    else
                    {
                        application.ApplicationName = "Redbook";
                        application.OrganizationId = 1;
                    }

                    await context.SaveChangesAsync();

                    // Load Seed Data for Users
                    var orgRole = await context.Roles.FindAsync(1);
                    if (orgRole == null)
                        await context.Roles.AddAsync(new Role
                        {
                            RoleName = "System Admin",
                            OrganizationId = 1,
                            IsSystemAdmin = true,
                            IsAdmin = true,
                            IsRetailer = true,
                        });
                    else
                    {
                        orgRole.RoleName = "System Admin";
                        orgRole.OrganizationId = 1;
                        orgRole.IsAdmin = true;
                        orgRole.IsSystemAdmin = true;
                        orgRole.IsRetailer = true;
                    }

                    await context.SaveChangesAsync();

                    var user = await context.Users.FindAsync("00000000-0000-0000-0000-000000000000");
                    if (user == null)
                        // Load Seed Data for Users
                        await context.Users.AddAsync(new User
                        {
                            UserId = new Guid().ToString(),
                            AccountBalance = int.MaxValue,
                            FirstName = "Md. Nafis",
                            LastName = "Sadik",
                            UserName = "nafis_sadik",
                            Email = "nafis_sadik@outlook.com",
                            Password = BCrypt.Net.BCrypt.EnhancedHashPassword("SHARMIN<3nafis23"),
                            Status = CommonConstants.StatusTypes.Active.ToString(),
                            RoleId = 1,
                            OrganizationId = 1,
                        });
                    else
                    {
                        user.UserId = "00000000-0000-0000-0000-000000000000";
                        user.AccountBalance = int.MaxValue;
                        user.FirstName = "Md. Nafis";
                        user.LastName = "Sadik";
                        user.UserName = "nafis_sadik";
                        user.Email = "nafis_sadik@outlook.com";
                        user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword("SHARMIN<3nafis23");
                        user.Status = CommonConstants.StatusTypes.Active.ToString();
                        user.RoleId = 1;
                        user.OrganizationId = 1;
                    }

                    await context.SaveChangesAsync();

                    var existingRoutes = await context.Routes.Where(r => r.RouteId > 1).ToListAsync();

                    context.Routes.RemoveRange(context.Routes);

                    var dashboardRoute = await context.Routes.AddAsync(
                        new Data.Entities.Route {
                            RouteName = "Dashboards",
                            Route1 = "/dashboard/home",
                            Description = "keypad",
                            ApplicationId = 1,
                            ParentRouteId = null,
                        });

                    await context.SaveChangesAsync();

                    var operationsRoute = await context.Routes.AddAsync(
                        new Data.Entities.Route
                        {
                            RouteName = "Business Operations",
                            Route1 = "",
                            Description = "layers",
                            ApplicationId = 1,
                            ParentRouteId = null,
                        });

                    await context.SaveChangesAsync();

                    await context.Routes.AddRangeAsync(new[] {
                        new Data.Entities.Route {
                            RouteName = "Purchase - Invoice",
                            Route1 = "/dashboard/purchase",
                            Description = "shopping-bag",
                            ApplicationId = 1,
                            ParentRouteId = operationsRoute.Entity.RouteId,
                        },
                        new Data.Entities.Route {
                            RouteName = "Purchase - Sales",
                            Route1 = "/dashboard/sales",
                            Description = "shopping-cart",
                            ApplicationId = 1,
                            ParentRouteId = operationsRoute.Entity.RouteId,
                        } 
                    });

                    await context.SaveChangesAsync();

                    var crmRoute = await context.Routes.AddAsync(
                        new Data.Entities.Route
                        {
                            RouteName = "CRM",
                            Route1 = "",
                            Description = "people",
                            ApplicationId = 1,
                            ParentRouteId = null,
                        });

                    await context.SaveChangesAsync();
                    
                    await context.Routes.AddAsync(
                        new Data.Entities.Route
                        {
                            RouteName = "Customers",
                            Route1 = "/dashboard/customers",
                            Description = "person",
                            ApplicationId = 1,
                            ParentRouteId = crmRoute.Entity.RouteId,
                        });

                    await context.SaveChangesAsync();

                    var settingsRoute = await context.Routes.AddAsync(
                        new Data.Entities.Route
                        {
                            RouteName = "Settings",
                            Route1 = "",
                            Description = "settings",
                            ApplicationId = 1,
                            ParentRouteId = null,
                        });

                    await context.SaveChangesAsync();

                    await context.Routes.AddAsync(
                        new Data.Entities.Route {
                            RouteName = "General Settings",
                            Route1 = "/dashboard/settings",
                            Description = "briefcase",
                            ApplicationId = 1,
                            ParentRouteId = settingsRoute.Entity.RouteId,
                        });

                    var productManagementRoute = await context.Routes.AddAsync(
                        new Data.Entities.Route {
                            RouteName = "Product Management",
                            Route1 = "",
                            Description = "cube",
                            ApplicationId = 1,
                            ParentRouteId = settingsRoute.Entity.RouteId,
                        });

                    await context.SaveChangesAsync();

                    await context.Routes.AddRangeAsync(new[] {
                        new Data.Entities.Route {
                            RouteName = "Categories",
                            Route1 = "/dashboard/categories",
                            Description = "cube",
                            ApplicationId = 1,
                            ParentRouteId = productManagementRoute.Entity.RouteId,
                        },
                        new Data.Entities.Route {
                            RouteName = "Products",
                            Route1 = "/dashboard/products",
                            Description = "cube",
                            ApplicationId = 1,
                            ParentRouteId = productManagementRoute.Entity.RouteId,
                        }
                    });

                    await context.SaveChangesAsync();

                    await context.Routes.AddRangeAsync(new[] {
                        new Data.Entities.Route {
                            RouteName = "Onboarding",
                            Route1 = "/dashboard/onboarding",
                            Description = "person-add",
                            ApplicationId = 1,
                            ParentRouteId = null,
                        },
                        new Data.Entities.Route {
                            RouteName = "Platform Settings",
                            Route1 = "/dashboard/platform-settings",
                            Description = "settings-2",
                            ApplicationId = 1,
                            ParentRouteId = null,
                        },
                    } );

                    await context.SaveChangesAsync();

                    await context.DisposeAsync();
                }
            }
        }
    }
}
