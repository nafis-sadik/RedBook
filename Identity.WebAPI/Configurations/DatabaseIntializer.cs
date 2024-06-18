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
                    context.Database.Migrate();

                SeedData(context);
            }
        }

        private static async void SeedData(RedbookIdentityContext context)
        {
            context.Database.EnsureCreated();

            #region Applications
            Application? redBookFrontEnd = context.Applications.FirstOrDefault(x => x.ApplicationName == GenericConstants.RedBookFrontEnd.ApplicationName);
            if (redBookFrontEnd == null)
            {
                redBookFrontEnd = context.Applications.Add(new Application
                {
                    ApplicationName = GenericConstants.RedBookFrontEnd.ApplicationName,
                    ApplicationUrl = GenericConstants.RedBookFrontEnd.ApplicationUrl,
                }).Entity;
                context.SaveChanges();
            }
            GenericConstants.RedBookFrontEnd.ApplicationId = redBookFrontEnd.ApplicationId;

            Application? blumeIdentity = context.Applications.FirstOrDefault(x => x.ApplicationName == GenericConstants.BlumeIdentity.ApplicationName);
            if (blumeIdentity == null)
            {
                blumeIdentity = context.Applications.Add(new Application
                {
                    ApplicationName = GenericConstants.BlumeIdentity.ApplicationName,
                    ApplicationUrl = GenericConstants.BlumeIdentity.ApplicationUrl,
                }).Entity;
                context.SaveChanges();
            }
            GenericConstants.BlumeIdentity.ApplicationId = blumeIdentity.ApplicationId;

            Application? redbookAPI = context.Applications.FirstOrDefault(x => x.ApplicationName == GenericConstants.RedbookAPI.ApplicationName);
            if (redbookAPI == null)
            {
                redbookAPI = context.Applications.Add(new Application
                {
                    ApplicationName = GenericConstants.RedbookAPI.ApplicationName,
                    ApplicationUrl = GenericConstants.RedbookAPI.ApplicationUrl,
                }).Entity;
                context.SaveChanges();
            }
            GenericConstants.RedbookAPI.ApplicationId = redbookAPI.ApplicationId;
            #endregion

            #region Organizations
            Organization? org = context.Organizations.FirstOrDefault(x => x.OrganizationName == "Blume Digital Corp.");
            if (org == null)
            {
                org = context.Organizations.Add(new Organization
                {
                    OrganizationName = "Blume Digital Corp.",
                    Address = "H # 69, Hollan Road, Islam Bug, Dakshin Khan, Uttara, Dhaka-1230",
                    LogoUrl = "https://picsum.photos/seed/picsum/250/400",
                    CreateDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdateDate = null,
                    UpdatededBy = string.Empty,
                }).Entity;

                context.SaveChanges();
            }
            #endregion

            #region User
            User? sysAdminUser = context.Users.FirstOrDefault(x => x.FirstName == "Md. Nafis" && x.LastName == "Sadik" && x.UserName == "nafis_sadik");
            if (sysAdminUser == null)
            {
                sysAdminUser = context.Users.Add(new User
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
                }).Entity;

                context.SaveChanges();
            }
            #endregion

            #region User Insert
            if (!context.UserRoleMappings.Any())
            {
                context.UserRoleMappings.Add(new UserRoleMapping
                {
                    UserId = sysAdminUser.UserId,
                    Role = new Role
                    {
                        RoleName = "System Admin",
                        IsAdmin = true,
                        IsRetailer = true,
                        IsSystemAdmin = true,
                        IsOwner = true,
                        OrganizationId = org.OrganizationId,
                        ApplicationId = redBookFrontEnd.ApplicationId,
                    },
                    OrganizationId = org.OrganizationId,
                });

                context.SaveChanges();
            }
            #endregion

            #region Roles
            Role? sysAdmin = context.Roles.FirstOrDefault(x => x.RoleName == RoleConstants.SystemAdmin.RoleName);
            if (sysAdmin == null)
            {
                sysAdmin = context.Roles.AddAsync(RoleConstants.SystemAdmin).Result.Entity;
                context.SaveChanges();
            }
            RoleConstants.SystemAdmin.RoleId = sysAdmin.RoleId;

            Role? redbookAdmin = context.Roles.FirstOrDefault(x => x.RoleName == RoleConstants.RedbookAdmin.RoleName);
            if (redbookAdmin == null)
            {
                redbookAdmin = context.Roles.AddAsync(RoleConstants.RedbookAdmin).Result.Entity;
                context.SaveChanges();
            }
            RoleConstants.RedbookAdmin.RoleId = redbookAdmin.RoleId;

            Role? redbookOwnerAdmin = context.Roles.FirstOrDefault(x => x.RoleName == RoleConstants.OwnerAdmin.RoleName);
            if (redbookOwnerAdmin == null)
            {
                redbookOwnerAdmin = context.Roles.AddAsync(RoleConstants.OwnerAdmin).Result.Entity;
                context.SaveChanges();
            }
            RoleConstants.OwnerAdmin.RoleId = redbookOwnerAdmin.RoleId;

            Role? retailer = context.Roles.FirstOrDefault(x => x.RoleName == RoleConstants.Retailer.RoleName);
            if (retailer == null)
            {
                retailer = context.Roles.AddAsync(RoleConstants.Retailer).Result.Entity;
                context.SaveChanges();
            }
            RoleConstants.Retailer.RoleId = retailer.RoleId;
            #endregion

            #region Route types
            if (!context.RouteTypes.Any())
            {
                RouteTypeConsts.GenericRoute = context.RouteTypes.Add(RouteTypeConsts.GenericRoute).Entity;
                context.SaveChanges();
                RouteTypeConsts.AdminRoute = context.RouteTypes.Add(RouteTypeConsts.AdminRoute).Entity;
                context.SaveChanges();
                RouteTypeConsts.RetailerRoute = context.RouteTypes.Add(RouteTypeConsts.RetailerRoute).Entity;
                context.SaveChanges();
                RouteTypeConsts.SysAdminRoute = context.RouteTypes.Add(RouteTypeConsts.SysAdminRoute).Entity;
                context.SaveChanges();
                RouteTypeConsts.OrganizationOwner = context.RouteTypes.Add(RouteTypeConsts.OrganizationOwner).Entity;
                context.SaveChanges();
            }
            else
            {
                RouteTypeConsts.GenericRoute = context.RouteTypes.FirstOrDefault(x => x.RouteTypeName == RouteTypeConsts.GenericRoute.RouteTypeName)!;
                RouteTypeConsts.AdminRoute = context.RouteTypes.FirstOrDefault(x => x.RouteTypeName == RouteTypeConsts.AdminRoute.RouteTypeName)!;
                RouteTypeConsts.RetailerRoute = context.RouteTypes.FirstOrDefault(x => x.RouteTypeName == RouteTypeConsts.RetailerRoute.RouteTypeName)!;
                RouteTypeConsts.SysAdminRoute = context.RouteTypes.FirstOrDefault(x => x.RouteTypeName == RouteTypeConsts.SysAdminRoute.RouteTypeName)!;
                RouteTypeConsts.OrganizationOwner = context.RouteTypes.FirstOrDefault(x => x.RouteTypeName == RouteTypeConsts.OrganizationOwner.RouteTypeName)!;
            }
            #endregion

            #region UI Routes (Menu)
            if (!context.Routes.Any())
            {
                context.Routes.RemoveRange(context.Routes);

                Route dashboardRoute = context.Routes.Add(
                    new Route
                    {
                        RouteName = "Dashboards",
                        RoutePath = "/dashboard/home",
                        Description = "keypad",
                        ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                        ParentRouteId = null,
                        RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                        IsMenuRoute = true
                    }).Entity;

                context.SaveChanges();

                Route operationsRoute = context.Routes.AddAsync(
                    new Route
                    {
                        RouteName = "Business Operations",
                        RoutePath = "",
                        Description = "layers",
                        ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                        ParentRouteId = null,
                        RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                        IsMenuRoute = true
                    }).Result.Entity;

                context.SaveChanges();

                context.Routes.AddRange(new[] {
                                new Route {
                                    RouteName = "Invoice - Purchase",
                                    RoutePath = "/dashboard/purchase",
                                    Description = "shopping-bag",
                                    ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                                    ParentRouteId = operationsRoute.RouteId,
                                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                                    IsMenuRoute = true
                                },
                                new Data.Entities.Route {
                                    RouteName = "Invoice - Sales",
                                    RoutePath = "/dashboard/sales",
                                    Description = "shopping-cart",
                                    ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                                    ParentRouteId = operationsRoute.RouteId,
                                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                                    IsMenuRoute = true
                                }
                            });

                context.SaveChanges();

                Route crmRoute = context.Routes.Add(
                    new Route
                    {
                        RouteName = "CRM",
                        RoutePath = "",
                        Description = "people",
                        ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                        ParentRouteId = null,
                        RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                        IsMenuRoute = true
                    }).Entity;

                context.SaveChanges();

                context.Routes.Add(new Route
                {
                    RouteName = "Customers",
                    RoutePath = "/dashboard/customers",
                    Description = "person",
                    ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                    ParentRouteId = crmRoute.RouteId,
                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                    IsMenuRoute = true
                });

                context.SaveChanges();

                Route settingsRoute = context.Routes.Add(new Route
                {
                    RouteName = "Settings",
                    RoutePath = "",
                    Description = "settings",
                    ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                    ParentRouteId = null,
                    RouteTypeId = RouteTypeConsts.AdminRoute.RouteTypeId,
                    IsMenuRoute = true
                }).Entity;

                context.SaveChanges();

                context.Routes.AddRange(new[] {
                                new Route {
                                    RouteName = "General Settings",
                                    RoutePath = "/dashboard/settings",
                                    Description = "briefcase",
                                    ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                                    ParentRouteId = settingsRoute.RouteId,
                                    RouteTypeId = RouteTypeConsts.AdminRoute.RouteTypeId,
                                    IsMenuRoute = true
                                },
                                new Route {
                                    RouteName = "Product List",
                                    RoutePath = "/dashboard/products",
                                    Description = "cube",
                                    ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                                    ParentRouteId = settingsRoute.RouteId,
                                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                                    IsMenuRoute = true
                                },
                                new Data.Entities.Route {
                                    RouteName = "Product Settings",
                                    RoutePath = "/dashboard/product-settings",
                                    Description = "options-2",
                                    ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                                    ParentRouteId = settingsRoute.RouteId,
                                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                                    IsMenuRoute = true
                                }
                            });

                context.SaveChanges();

                context.Routes.AddRange(new[] {
                                new Route {
                                    RouteName = "Onboarding",
                                    RoutePath = "/dashboard/onboarding",
                                    Description = "person-add",
                                    ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                                    ParentRouteId = null,
                                    RouteTypeId = RouteTypeConsts.RetailerRoute.RouteTypeId,
                                    IsMenuRoute = true
                                },
                                new Route {
                                    RouteName = "Platform Settings",
                                    RoutePath = "/dashboard/platform-settings",
                                    Description = "settings-2",
                                    ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                                    ParentRouteId = null,
                                    RouteTypeId = RouteTypeConsts.SysAdminRoute.RouteTypeId,
                                    IsMenuRoute = true
                                },
                            });

                context.SaveChanges();
            }
            #endregion

            context.Dispose();
        }
    }
}