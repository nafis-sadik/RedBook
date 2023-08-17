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
                        context.Organizations.Load();
                        context.Users.Load();

                        // Load Seed Data for Organizations
                        var org = context.Organizations.Find(1);
                        if (org == null)
                            context.Organizations.AddAsync(new Organization
                            {
                                OrganizationName = "Blume Digital Corp."
                            });
                        else
                            org.OrganizationName = "Blume Digital Corp.";

                        context.SaveChanges();

                        // Load Seed Data for Users
                        var orgRole = context.Roles.Find(1);
                        if (orgRole == null)
                            context.Roles.AddAsync(new Role
                            {
                                RoleName = "System Admin",
                                OrganizationId = 1,
                                IsAdminRole = 1,
                            });
                        else
                        {
                            orgRole.RoleName = "System Admin";
                            orgRole.OrganizationId = 1;
                            orgRole.IsAdminRole = 1;
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
