using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Data
{
    public static class DomainExtension
    {
        public static void AddDatabaseConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RedBookInventoryContext>(opts =>
            {
                opts.UseMySQL(
                    configuration["ConnectionStrings:Inventory"]
                    //, sqlOpts => sqlOpts.MigrationsHistoryTable("__EFMigrationsHistory", RedBookInventoryContext.DefaultSchema).UseRelationalNulls()
                );
            });
        }
    }
}
