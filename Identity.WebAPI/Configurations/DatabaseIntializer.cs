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

            #region Create SysAdmin User
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

            #region Roles
            Role? sysAdmin = context.Roles.FirstOrDefault(x => x.RoleName == RoleConstants.SystemAdmin.RoleName);
            if (sysAdmin == null)
            {
                sysAdmin = context.Roles.Add(RoleConstants.SystemAdmin).Entity;
                context.SaveChanges();
            }
            RoleConstants.SystemAdmin.RoleId = sysAdmin.RoleId;

            Role? redbookOwnerAdmin = context.Roles.FirstOrDefault(x => x.RoleName == RoleConstants.Owner.RoleName);
            if (redbookOwnerAdmin == null)
            {
                redbookOwnerAdmin = context.Roles.Add(RoleConstants.Owner).Entity;
                context.SaveChanges();
            }
            RoleConstants.Owner.RoleId = redbookOwnerAdmin.RoleId;

            Role? retailer = context.Roles.FirstOrDefault(x => x.RoleName == RoleConstants.Retailer.RoleName);
            if (retailer == null)
            {
                RoleConstants.Retailer.OrganizationId = org.OrganizationId;
                retailer = context.Roles.Add(RoleConstants.Retailer).Entity;
                context.SaveChanges();
            }
            RoleConstants.Retailer.RoleId = retailer.RoleId;
            #endregion

            #region User Role Mapping
            if (!context.UserRoleMappings.Any())
            {
                context.UserRoleMappings.AddRange(new UserRoleMapping
                {
                    UserId = sysAdminUser.UserId,
                    RoleId = sysAdmin.RoleId,
                    OrganizationId = org.OrganizationId,
                    CreateDate = DateTime.UtcNow,
                    CreateBy = sysAdminUser.UserId,
                    UpdateBy = null,
                    UpdateDate = DateTime.UtcNow,
                });

                context.SaveChanges();
            }
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
            }
            else
            {
                RouteTypeConsts.GenericRoute = context.RouteTypes.FirstOrDefault(x => x.RouteTypeName == RouteTypeConsts.GenericRoute.RouteTypeName)!;
                RouteTypeConsts.AdminRoute = context.RouteTypes.FirstOrDefault(x => x.RouteTypeName == RouteTypeConsts.AdminRoute.RouteTypeName)!;
                RouteTypeConsts.RetailerRoute = context.RouteTypes.FirstOrDefault(x => x.RouteTypeName == RouteTypeConsts.RetailerRoute.RouteTypeName)!;
                RouteTypeConsts.SysAdminRoute = context.RouteTypes.FirstOrDefault(x => x.RouteTypeName == RouteTypeConsts.SysAdminRoute.RouteTypeName)!;
            }
            #endregion

            List<Route> routes = new List<Route>();
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
                routes.Add(dashboardRoute);

                Route operationsParentRoute = context.Routes.Add(
                    new Route
                    {
                        RouteName = "Business Operations",
                        RoutePath = "",
                        Description = "layers",
                        ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                        ParentRouteId = null,
                        RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                        IsMenuRoute = true
                    }).Entity;
                context.SaveChanges();
                routes.Add(operationsParentRoute);

                List<Route> operationsRoute = new List<Route> {
                                new Route {
                                    RouteName = "Invoice - Purchase",
                                    RoutePath = "/dashboard/purchase",
                                    Description = "shopping-bag",
                                    ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                                    ParentRouteId = operationsParentRoute.RouteId,
                                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                                    IsMenuRoute = true
                                },
                                new Route {
                                    RouteName = "Invoice - Sales",
                                    RoutePath = "/dashboard/sales",
                                    Description = "shopping-cart",
                                    ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                                    ParentRouteId = operationsParentRoute.RouteId,
                                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                                    IsMenuRoute = true
                                },
                                new Route
                                {
                                    RouteName = "Vendors",
                                    RoutePath = "/dashboard/vendors",
                                    Description = "people",
                                    ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                                    ParentRouteId = operationsParentRoute.RouteId,
                                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                                    IsMenuRoute = true
                                }
                            };
                context.Routes.AddRange(operationsRoute);
                context.SaveChanges();
                routes.AddRange(operationsRoute);

                Route crmParentRoute = context.Routes.Add(
                    new Route
                    {
                        RouteName = "CRM",
                        RoutePath = "",
                        Description = "phone-call",
                        ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                        ParentRouteId = null,
                        RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                        IsMenuRoute = true
                    }).Entity;
                context.SaveChanges();
                routes.Add(crmParentRoute);

                List<Route> crmRoute = new List<Route> {
                    new Route
                    {
                        RouteName = "Customers",
                        RoutePath = "/dashboard/customers",
                        Description = "person",
                        ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                        ParentRouteId = crmParentRoute.RouteId,
                        RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                        IsMenuRoute = true
                    }
                };
                context.Routes.AddRange(crmRoute);
                context.SaveChanges();
                routes.AddRange(crmRoute);

                Route settingsParentRoute = context.Routes.Add(new Route
                {
                    RouteName = "Settings",
                    RoutePath = "",
                    Description = "settings",
                    ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                    ParentRouteId = null,
                    RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                    IsMenuRoute = true
                }).Entity;
                context.SaveChanges();
                routes.Add(settingsParentRoute);

                List<Route> settingsRoutes = new List<Route> {
                    new Route {
                        RouteName = "General Settings",
                        RoutePath = "/dashboard/settings",
                        Description = "briefcase",
                        ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                        ParentRouteId = settingsParentRoute.RouteId,
                        RouteTypeId = RouteTypeConsts.AdminRoute.RouteTypeId,
                        IsMenuRoute = true
                    },
                    new Route {
                        RouteName = "Product List",
                        RoutePath = "/dashboard/products",
                        Description = "cube",
                        ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                        ParentRouteId = settingsParentRoute.RouteId,
                        RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                        IsMenuRoute = true
                    },
                    new Route {
                        RouteName = "Product Settings",
                        RoutePath = "/dashboard/product-settings",
                        Description = "options-2",
                        ApplicationId = GenericConstants.RedBookFrontEnd.ApplicationId,
                        ParentRouteId = settingsParentRoute.RouteId,
                        RouteTypeId = RouteTypeConsts.GenericRoute.RouteTypeId,
                        IsMenuRoute = true
                    }
                };
                context.Routes.AddRange(settingsRoutes);
                context.SaveChanges();
                routes.AddRange(settingsRoutes);

                List<Route> othersRoutes = new List<Route> {
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
                };
                context.Routes.AddRange(othersRoutes);
                context.SaveChanges();
                routes.AddRange(othersRoutes);
            }
            #endregion

            #region Role Route Mapping for SysAdmin
            List<RoleRouteMapping> roleRouteMappings = new List<RoleRouteMapping>();
            // Sys Admin Mapping
            foreach (Route route in routes)
            {
                roleRouteMappings.Add(new RoleRouteMapping
                {
                    RouteId = route.RouteId,
                    RoleId = sysAdmin.RoleId,
                    HasCreateAccess = true,
                    HasDeleteAccess = true,
                    HasUpdateAccess = true,
                });
            }
            Route? OnboardingRoute = routes.FirstOrDefault(x => x.RouteName == "Onboarding");
            // Retailer Route Mapping
            if(OnboardingRoute != null)
            {
                roleRouteMappings.Add(new RoleRouteMapping
                {
                    RoleId = retailer.RoleId,
                    RouteId = OnboardingRoute.RouteId,
                    HasCreateAccess = false,
                    HasDeleteAccess = false,
                    HasUpdateAccess = false,
                });
            }
            context.RoleRouteMappings.AddRange(roleRouteMappings);
            context.SaveChanges();
            #endregion

            context.Dispose();
        }
    }
}