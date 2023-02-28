using Identity.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.WebAPI.Configurations
{
    public static class DatabaseIntializer
    {
        public static void InitDatabase(this IApplicationBuilder app, IWebHostEnvironment env)
        {

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UserManagementSystemContext>();

                // Only run database migrations in development environment
                if (env.IsDevelopment())
                {
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                }
            }
        }
    }
}
