using Identity.Data.CommonConstant;
using Identity.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RedBook.Core.Constants;

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

                // Only run database migrations in development environment
                if (env.IsDevelopment())
                {
                    context.Database.EnsureCreated();

                    if(!context.UserRoleMappings.Any())
                        context.UserRoleMappings.Add(new UserRoleMapping {
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

                    context.SaveChanges();
                    //if (context.Database.GetPendingMigrations().Any())
                    //{
                    //    context.Database.Migrate();

                    //    #region Organization
                    //    if (await context.Organizations.Where(x => x.OrganizationName == "Blume Digital Corp.").CountAsync() <= 0)
                    //    {
                    //        var orgs = await context.Organizations.Where(x => x.OrganizationId > 0).ToListAsync();
                    //        context.Organizations.RemoveRange(orgs);

                    //        // Load Seed Data for Organizations
                    //        await context.Organizations.AddAsync(new Organization
                    //        {
                    //            OrganizationName = "Blume Digital Corp.",
                    //            Address = "H # 69, Hollan Road, Islam Bug, Dakshin Khan, Uttara, Dhaka-1230",
                    //            LogoUrl = "https://picsum.photos/seed/picsum/250/400",
                    //            CreateDate = DateTime.UtcNow,
                    //            CreatedBy = 1,
                    //            UpdateDate = null,
                    //            UpdatededBy = string.Empty,
                    //        });
                    //        await context.SaveChangesAsync();
                    //    }
                    //    #endregion

                    //    #region Route Type Seed
                    //    EntityEntry<RouteType> entityEntryRoute = await context.RouteTypes.AddAsync(new RouteType
                    //    {
                    //        RouteTypeName = RouteTypeConsts.GenericRoute.RouteTypeName
                    //    });
                    //    await context.SaveChangesAsync();

                    //    if (entityEntryRoute != null)
                    //        RouteTypeConsts.GenericRoute = entityEntryRoute.Entity;
                    //    else
                    //        throw new ArgumentException("Data Insertion Failed");

                    //    entityEntryRoute = await context.RouteTypes.AddAsync(new RouteType
                    //    {
                    //        RouteTypeName = RouteTypeConsts.AdminRoute.RouteTypeName
                    //    });
                    //    await context.SaveChangesAsync();

                    //    if (entityEntryRoute != null)
                    //        RouteTypeConsts.AdminRoute = entityEntryRoute.Entity;
                    //    else
                    //        throw new ArgumentException("Data Insertion Failed");

                    //    entityEntryRoute = await context.RouteTypes.AddAsync(new RouteType
                    //    {
                    //        RouteTypeName = RouteTypeConsts.RetailerRoute.RouteTypeName
                    //    });
                    //    await context.SaveChangesAsync();

                    //    if (entityEntryRoute != null)
                    //        RouteTypeConsts.RetailerRoute = entityEntryRoute.Entity;
                    //    else
                    //        throw new ArgumentException("Data Insertion Failed");

                    //    entityEntryRoute = await context.RouteTypes.AddAsync(new RouteType
                    //    {
                    //        RouteTypeName = RouteTypeConsts.SysAdminRoute.RouteTypeName
                    //    });
                    //    await context.SaveChangesAsync();

                    //    if (entityEntryRoute != null)
                    //        RouteTypeConsts.SysAdminRoute = entityEntryRoute.Entity;
                    //    else
                    //        throw new ArgumentException("Data Insertion Failed");
                    //    #endregion

                    //    #region Applications
                    //    var blumeIdentity = new Application
                    //    {
                    //        ApplicationName = "Blume Identity",
                    //        OrganizationId = 1,
                    //        ApplicationUrl = "http://localhost:5062"
                    //    };
                    //    var redbookAPI = new Application
                    //    {
                    //        ApplicationName = "Redbook API",
                    //        OrganizationId = 1,
                    //        ApplicationUrl = "http://localhost:7238"
                    //    };
                    //    var redbookFrontend = new Application
                    //    {
                    //        ApplicationName = "Redbook Frontend",
                    //        OrganizationId = 1,
                    //        ApplicationUrl = "http://localhost:4200"
                    //    };
                    //    await context.Applications.AddRangeAsync(blumeIdentity, redbookAPI, redbookFrontend);
                    //    await context.SaveChangesAsync();
                    //    #endregion

                    //    #region Users Role
                    //    Role? sysAdminRole = await context.Roles.FindAsync(1);
                    //    if (sysAdminRole == null)
                    //    {
                    //        sysAdminRole = new Role
                    //        {
                    //            RoleName = "System Admin",
                    //            OrganizationId = 1,
                    //            IsSystemAdmin = true,
                    //            IsAdmin = true,
                    //            IsRetailer = true,
                    //        };
                    //        await context.Roles.AddAsync(sysAdminRole);
                    //    }
                    //    else
                    //    {
                    //        sysAdminRole.RoleName = "System Admin";
                    //        sysAdminRole.OrganizationId = 1;
                    //        sysAdminRole.IsAdmin = true;
                    //        sysAdminRole.IsSystemAdmin = true;
                    //        sysAdminRole.IsRetailer = true;
                    //    }

                    //    await context.SaveChangesAsync();
                    //    #endregion

                    //    #region Role Mapping
                    //    await context.UserRoles.AddAsync(new UserRole
                    //    {
                    //        RoleId = sysAdminRole.RoleId,
                    //        UserId = user.UserId
                    //    });
                    //    await context.SaveChangesAsync();
                    //    #endregion
                    //    // Route types
                    //    //if (await context.RouteTypes.Where(x => x.RouteTypeId > 0).CountAsync() <= 0)
                    //    //{
                    //    //    // Make seed user sys admin
                    //    //    List<UserRole> userRoleMapping = await context.UserRoles.Where(x => x.UserRoleId > 0).ToListAsync();
                    //    //    context.RemoveRange(userRoleMapping);
                    //    //    await context.SaveChangesAsync();
                           

                    //    //    var existingRoutes = await context.Routes.Where(r => r.RouteId > 1).ToListAsync();
                    //    //    if (!existingRoutes.Any())
                    //    //    {
                    //    //        context.Routes.RemoveRange(context.Routes);

                    //    //        var dashboardRoute = await context.Routes.AddAsync(
                    //    //            new Data.Entities.Route
                    //    //            {
                    //    //                RouteName = "Dashboards",
                    //    //                Route1 = "/dashboard/home",
                    //    //                Description = "keypad",
                    //    //                ApplicationId = redbookFrontend.ApplicationId,
                    //    //                ParentRouteId = null,
                    //    //                RouteTypesId = RouteTypeConsts.GenericRoute.RouteTypeId
                    //    //            });

                    //    //        await context.SaveChangesAsync();

                    //    //        var operationsRoute = await context.Routes.AddAsync(
                    //    //            new Data.Entities.Route
                    //    //            {
                    //    //                RouteName = "Business Operations",
                    //    //                Route1 = "",
                    //    //                Description = "layers",
                    //    //                ApplicationId = redbookFrontend.ApplicationId,
                    //    //                ParentRouteId = null,
                    //    //                RouteTypesId = RouteTypeConsts.GenericRoute.RouteTypeId
                    //    //            });

                    //    //        await context.SaveChangesAsync();

                    //    //        await context.Routes.AddRangeAsync(new[] {
                    //    //            new Data.Entities.Route {
                    //    //                RouteName = "Invoice - Purchase",
                    //    //                Route1 = "/dashboard/purchase",
                    //    //                Description = "shopping-bag",
                    //    //                ApplicationId = redbookFrontend.ApplicationId,
                    //    //                ParentRouteId = operationsRoute.Entity.RouteId,
                    //    //                RouteTypesId = RouteTypeConsts.GenericRoute.RouteTypeId
                    //    //            },
                    //    //            new Data.Entities.Route {
                    //    //                RouteName = "Invoice - Sales",
                    //    //                Route1 = "/dashboard/sales",
                    //    //                Description = "shopping-cart",
                    //    //                ApplicationId = redbookFrontend.ApplicationId,
                    //    //                ParentRouteId = operationsRoute.Entity.RouteId,
                    //    //                RouteTypesId = RouteTypeConsts.GenericRoute.RouteTypeId
                    //    //            }
                    //    //        });

                    //    //        await context.SaveChangesAsync();

                    //    //        var crmRoute = await context.Routes.AddAsync(
                    //    //        new Data.Entities.Route
                    //    //        {
                    //    //            RouteName = "CRM",
                    //    //            Route1 = "",
                    //    //            Description = "people",
                    //    //            ApplicationId = redbookFrontend.ApplicationId,
                    //    //            ParentRouteId = null,
                    //    //            RouteTypesId = RouteTypeConsts.GenericRoute.RouteTypeId
                    //    //        });

                    //    //        await context.SaveChangesAsync();

                    //    //        await context.Routes.AddAsync(new Data.Entities.Route
                    //    //        {
                    //    //            RouteName = "Customers",
                    //    //            Route1 = "/dashboard/customers",
                    //    //            Description = "person",
                    //    //            ApplicationId = redbookFrontend.ApplicationId,
                    //    //            ParentRouteId = crmRoute.Entity.RouteId,
                    //    //            RouteTypesId = RouteTypeConsts.GenericRoute.RouteTypeId
                    //    //        });

                    //    //        await context.SaveChangesAsync();

                    //    //        var settingsRoute = await context.Routes.AddAsync(new Data.Entities.Route
                    //    //        {
                    //    //            RouteName = "Settings",
                    //    //            Route1 = "",
                    //    //            Description = "settings",
                    //    //            ApplicationId = redbookFrontend.ApplicationId,
                    //    //            ParentRouteId = null,
                    //    //            RouteTypesId = RouteTypeConsts.AdminRoute.RouteTypeId
                    //    //        });

                    //    //        await context.SaveChangesAsync();

                    //    //        await context.Routes.AddRangeAsync(new[] {
                    //    //            new Data.Entities.Route {
                    //    //                RouteName = "General Settings",
                    //    //                Route1 = "/dashboard/settings",
                    //    //                Description = "briefcase",
                    //    //                ApplicationId = redbookFrontend.ApplicationId,
                    //    //                ParentRouteId = settingsRoute.Entity.RouteId,
                    //    //                RouteTypesId = RouteTypeConsts.AdminRoute.RouteTypeId
                    //    //            },
                    //    //            new Data.Entities.Route {
                    //    //                RouteName = "Product List",
                    //    //                Route1 = "/dashboard/products",
                    //    //                Description = "cube",
                    //    //                ApplicationId = redbookFrontend.ApplicationId,
                    //    //                ParentRouteId = settingsRoute.Entity.RouteId,
                    //    //                RouteTypesId = RouteTypeConsts.GenericRoute.RouteTypeId
                    //    //            },
                    //    //            new Data.Entities.Route {
                    //    //                RouteName = "Product Settings",
                    //    //                Route1 = "/dashboard/product-settings",
                    //    //                Description = "options-2",
                    //    //                ApplicationId = redbookFrontend.ApplicationId,
                    //    //                ParentRouteId = settingsRoute.Entity.RouteId,
                    //    //                RouteTypesId = RouteTypeConsts.GenericRoute.RouteTypeId
                    //    //            }
                    //    //        });

                    //    //        await context.SaveChangesAsync();

                    //    //        await context.Routes.AddRangeAsync(new[] {
                    //    //            new Data.Entities.Route {
                    //    //                RouteName = "Onboarding",
                    //    //                Route1 = "/dashboard/onboarding",
                    //    //                Description = "person-add",
                    //    //                ApplicationId = redbookFrontend.ApplicationId,
                    //    //                ParentRouteId = null,
                    //    //                RouteTypesId = RouteTypeConsts.RetailerRoute.RouteTypeId
                    //    //            },
                    //    //            new Data.Entities.Route {
                    //    //                RouteName = "Platform Settings",
                    //    //                Route1 = "/dashboard/platform-settings",
                    //    //                Description = "settings-2",
                    //    //                ApplicationId = redbookFrontend.ApplicationId,
                    //    //                ParentRouteId = null,
                    //    //                RouteTypesId = RouteTypeConsts.SysAdminRoute.RouteTypeId
                    //    //            },
                    //    //        });

                    //    //        await context.SaveChangesAsync();
                    //    //    }

                    //    //    await context.DisposeAsync();
                    //    //}
                    //}
                }
            }
        }
    }
}
