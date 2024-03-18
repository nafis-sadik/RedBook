using Identity.Data.CommonConstant;
using Identity.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Route = Identity.Data.Entities.Route;

namespace Identity.WebAPI.Configurations
{
    /// <summary>
    /// This class creates database if it doesn't exist and initializes database with seed data
    /// </summary>
    public static class DatabaseIntializer
    {
        /// <summary>
        /// Creates database if it doesn't exist and initializes database with seed data
        /// </summary>
        public static async void InitDatabase(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                RedbookIdentityContext context = scope.ServiceProvider.GetRequiredService<RedbookIdentityContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                    // Only run database migrations in development environment
                    if (env.IsDevelopment())
                    {
                        context.Database.EnsureCreated();

                        Application? redbookFrontend = null;

                        if (!context.UserRoleMappings.Any())
                            context.UserRoleMappings.Add(new UserRoleMapping
                            {
                                User = new User
                                {
                                    FirstName = "Md. Nafis",
                                    LastName = "Sadik",
                                    UserName = "nafis_sadik",
                                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword("SHARMIN<3nafis23"),
                                    Status = true,
                                    AccountBalance = int.MaxValue,
                                    Email = "nafis_sadik@outlook.com",
                                    Address = "H # 69, Hollan Road, Islam Bug, Dakshin Khan, Uttara, Dhaka-1230",
                                    PhoneNumber = "+8801628301510",
                                },
                                Role = new Role
                                {
                                    RoleName = "System Admin",
                                    IsAdmin = true,
                                    IsRetailer = true,
                                    IsSystemAdmin = true,
                                    IsOwner = true,
                                    Organization = new Organization
                                    {
                                        OrganizationName = "Blume Digital Corp.",
                                        Address = "H # 69, Hollan Road, Islam Bug, Dakshin Khan, Uttara, Dhaka-1230",
                                        LogoUrl = "https://picsum.photos/seed/picsum/250/400",
                                        CreateDate = DateTime.UtcNow,
                                        CreatedBy = 1,
                                        UpdateDate = null,
                                        UpdatededBy = string.Empty,
                                    }
                                }
                            });

                        if (context.Applications.Any())
                        {
                            redbookFrontend = context.Applications.FirstOrDefault(x =>
                                    x.ApplicationName == "Redbook Angular"
                                    && x.ApplicationUrl == "http://localhost:4200");
                        }

                        if (redbookFrontend == null)
                        {
                            redbookFrontend = context.Applications.Add(new Application
                            {
                                ApplicationName = "Redbook Angular",
                                ApplicationUrl = "http://localhost:4200",
                            }).Entity;

                            context.SaveChanges();

                            var blumeIdentity = new Application
                            {
                                ApplicationName = "Blume Identity",
                                ApplicationUrl = "http://localhost:5062"
                            };
                            var redbookAPI = new Application
                            {
                                ApplicationName = "Redbook API",
                                ApplicationUrl = "http://localhost:7238"
                            };

                            context.Applications.AddRange(blumeIdentity, redbookAPI);

                            context.SaveChanges();
                        }

                        // Route types
                        if (!context.RouteTypes.Any())
                        {
                            RouteTypeConsts.GenericRoute = context.RouteTypes.Add(RouteTypeConsts.GenericRoute).Entity;
                            await context.SaveChangesAsync();
                            RouteTypeConsts.AdminRoute = context.RouteTypes.Add(RouteTypeConsts.AdminRoute).Entity;
                            await context.SaveChangesAsync();
                            RouteTypeConsts.RetailerRoute = context.RouteTypes.Add(RouteTypeConsts.RetailerRoute).Entity;
                            await context.SaveChangesAsync();
                            RouteTypeConsts.SysAdminRoute = context.RouteTypes.Add(RouteTypeConsts.SysAdminRoute).Entity;
                            await context.SaveChangesAsync();
                            RouteTypeConsts.OrganizationOwner = context.RouteTypes.Add(RouteTypeConsts.OrganizationOwner).Entity;
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            RouteTypeConsts.GenericRoute = context.RouteTypes.FirstOrDefault(x => x.RouteTypeName == RouteTypeConsts.GenericRoute.RouteTypeName)!;
                            RouteTypeConsts.AdminRoute = context.RouteTypes.FirstOrDefault(x => x.RouteTypeName == RouteTypeConsts.AdminRoute.RouteTypeName)!;
                            RouteTypeConsts.RetailerRoute = context.RouteTypes.FirstOrDefault(x => x.RouteTypeName == RouteTypeConsts.RetailerRoute.RouteTypeName)!;
                            RouteTypeConsts.SysAdminRoute = context.RouteTypes.FirstOrDefault(x => x.RouteTypeName == RouteTypeConsts.SysAdminRoute.RouteTypeName)!;
                            RouteTypeConsts.OrganizationOwner = context.RouteTypes.FirstOrDefault(x => x.RouteTypeName == RouteTypeConsts.OrganizationOwner.RouteTypeName)!;
                        }

                        if (!context.Routes.Any())
                        {
                            context.Routes.RemoveRange(context.Routes);

                            var dashboardRoute = await context.Routes.AddAsync(
                                new Route
                                {
                                    RouteName = "Dashboards",
                                    Route1 = "/dashboard/home",
                                    Description = "keypad",
                                    ApplicationId = redbookFrontend.ApplicationId,
                                    ParentRouteId = null,
                                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId
                                });

                            await context.SaveChangesAsync();

                            var operationsRoute = await context.Routes.AddAsync(
                                new Route
                                {
                                    RouteName = "Business Operations",
                                    Route1 = "",
                                    Description = "layers",
                                    ApplicationId = redbookFrontend.ApplicationId,
                                    ParentRouteId = null,
                                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId
                                });

                            await context.SaveChangesAsync();

                            await context.Routes.AddRangeAsync(new[] {
                                new Route {
                                    RouteName = "Invoice - Purchase",
                                    Route1 = "/dashboard/purchase",
                                    Description = "shopping-bag",
                                    ApplicationId = redbookFrontend.ApplicationId,
                                    ParentRouteId = operationsRoute.Entity.RouteId,
                                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId
                                },
                                new Data.Entities.Route {
                                    RouteName = "Invoice - Sales",
                                    Route1 = "/dashboard/sales",
                                    Description = "shopping-cart",
                                    ApplicationId = redbookFrontend.ApplicationId,
                                    ParentRouteId = operationsRoute.Entity.RouteId,
                                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId
                                }
                            });

                            await context.SaveChangesAsync();

                            var crmRoute = await context.Routes.AddAsync(
                                new Route
                                {
                                    RouteName = "CRM",
                                    Route1 = "",
                                    Description = "people",
                                    ApplicationId = redbookFrontend.ApplicationId,
                                    ParentRouteId = null,
                                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId
                                });

                            await context.SaveChangesAsync();

                            await context.Routes.AddAsync(new Route
                            {
                                RouteName = "Customers",
                                Route1 = "/dashboard/customers",
                                Description = "person",
                                ApplicationId = redbookFrontend.ApplicationId,
                                ParentRouteId = crmRoute.Entity.RouteId,
                                RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId
                            });

                            await context.SaveChangesAsync();

                            var settingsRoute = await context.Routes.AddAsync(new Data.Entities.Route
                            {
                                RouteName = "Settings",
                                Route1 = "",
                                Description = "settings",
                                ApplicationId = redbookFrontend.ApplicationId,
                                ParentRouteId = null,
                                RouteTypeId = RouteTypeConsts.AdminRoute.RouteTypeId
                            });

                            await context.SaveChangesAsync();

                            await context.Routes.AddRangeAsync(new[] {
                                new Route {
                                    RouteName = "General Settings",
                                    Route1 = "/dashboard/settings",
                                    Description = "briefcase",
                                    ApplicationId = redbookFrontend.ApplicationId,
                                    ParentRouteId = settingsRoute.Entity.RouteId,
                                    RouteTypeId = RouteTypeConsts.AdminRoute.RouteTypeId
                                },
                                new Route {
                                    RouteName = "Product List",
                                    Route1 = "/dashboard/products",
                                    Description = "cube",
                                    ApplicationId = redbookFrontend.ApplicationId,
                                    ParentRouteId = settingsRoute.Entity.RouteId,
                                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId
                                },
                                new Data.Entities.Route {
                                    RouteName = "Product Settings",
                                    Route1 = "/dashboard/product-settings",
                                    Description = "options-2",
                                    ApplicationId = redbookFrontend.ApplicationId,
                                    ParentRouteId = settingsRoute.Entity.RouteId,
                                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId
                                }
                            });

                            await context.SaveChangesAsync();

                            await context.Routes.AddRangeAsync(new[] {
                                new Route {
                                    RouteName = "Onboarding",
                                    Route1 = "/dashboard/onboarding",
                                    Description = "person-add",
                                    ApplicationId = redbookFrontend.ApplicationId,
                                    ParentRouteId = null,
                                    RouteTypeId = RouteTypeConsts.RetailerRoute.RouteTypeId
                                },
                                new Route {
                                    RouteName = "Platform Settings",
                                    Route1 = "/dashboard/platform-settings",
                                    Description = "settings-2",
                                    ApplicationId = redbookFrontend.ApplicationId,
                                    ParentRouteId = null,
                                    RouteTypeId = RouteTypeConsts.SysAdminRoute.RouteTypeId
                                },
                            });

                            await context.SaveChangesAsync();
                        }

                        await context.DisposeAsync();
                    }
                }
            }
        }
    }
}