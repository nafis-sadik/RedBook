using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Identity.Data
{
    public static class DomainExtension
    {
        public static void AddDatabaseConfigurations(this IServiceCollection services, Action<DbContextOptionsBuilder> dbOptionsBuilder)
        {
            services.AddDbContextPool<UserManagementSystemContext>(dbOptionsBuilder);

            //services.AddDbContext<UserManagementSystemContext>(opts =>
            //{
            //    opts.UseMySQL(
            //        configuration["ConnectionStrings:Identity"]
            //        //, sqlOpts => sqlOpts.MigrationsHistoryTable("__EFMigrationsHistory", UserManagementSystemContext.DefaultSchema).UseRelationalNulls()
            //    );
            //});
        }
    }
}
