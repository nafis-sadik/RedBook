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
                        //context.Applications.Include(r => r.Routes).Load();
                        //context.Organizations.Load();
                        //context.Users.Load();

                        // Load Seed Data for Organizations
                        var org = context.Organizations.Find(1);
                        if (org == null)
                            context.Organizations.AddAsync(new Organization
                            {
                                OrganizationName = "Blume Digital Corp.",
                                CreateDate = DateTime.Now,
                                CreatedBy = "00000000-0000-0000-0000-000000000000",
                            });
                        else {
                            org.OrganizationName = "Blume Digital Corp.";
                            org.CreateDate = DateTime.Now;
                            org.CreatedBy = "00000000-0000-0000-0000-000000000000";
                        }

                        context.SaveChanges();

                        // Load Seed Data for Users
                        var orgRole = context.Roles.Find(1);
                        if (orgRole == null)
                            context.Roles.AddAsync(new Role
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

                        context.SaveChanges();

                        // Load Seed Data for Applications
                        var application = context.Applications.Find(1);
                        if(application == null)
                        {
                            context.Applications.AddAsync(new Application
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

                        context.SaveChanges();

                        var user = context.Users.Find("00000000-0000-0000-0000-000000000000");
                        if (user == null)
                            // Load Seed Data for Users
                            context.Users.AddAsync(new User
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

                        context.SaveChanges();

                        context.Dispose();
                    }
                }
            }
        }
    }
}
