using Inventory.Data;
using Inventory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.WebAPI.Configurations
{
    public static class DatabaseIntializer
    {
        public static void InitDatabase(this IApplicationBuilder app, IWebHostEnvironment env)
        {

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<RedbookInventoryContext>();

                // Only run database migrations in development environment
                if (env.IsDevelopment())
                {
                    if (context != null && context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                }
            } 
        }
    }
}
