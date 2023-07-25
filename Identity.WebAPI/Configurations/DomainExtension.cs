using Identity.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Data
{
    public static class DomainExtension
    {
        public static void AddDatabaseConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserManagementSystemContext>(opts =>
            {
                opts.UseMySQL(
                    configuration["ConnectionStrings:Identity"],
                    //sqlOpts => sqlOpts.MigrationsHistoryTable("__EFMigrationsHistory").UseRelationalNulls()
                    //b => b.MigrationsAssembly("Identity")
                    sqlOpts => sqlOpts.MigrationsHistoryTable("__EFMigrationsHistory", UserManagementSystemContext.DefaultSchema).UseRelationalNulls()
                );
            });
        }
    }
}
