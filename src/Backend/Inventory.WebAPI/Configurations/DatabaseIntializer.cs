﻿using Inventory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

namespace Inventory.WebAPI.Configurations
{
    /// <summary>
    /// This class creates database if it doesn't exist and initializes database with seed data
    /// </summary>
    public static class DatabaseIntializer
    {
        /// <summary>
        /// Creates database if it doesn't exist and initializes database with seed data
        /// </summary>
        public static void InitDatabase(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                RedbookInventoryContext context = scope.ServiceProvider.GetRequiredService<RedbookInventoryContext>();

                // Only run database migrations in development environment
                if (env.IsDevelopment())
                {
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }

                    SeedData(context);
                }
            }
        }

        private static async void SeedData(RedbookInventoryContext context)
        {
            context.Database.EnsureCreated();

            context.SaveChanges();

            context.Dispose();
        }
    }
}
