using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RedBook.Core.AutoMapper;
using RedBook.Core.EntityFramework;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace RedBook.Core
{
    public static class CoreDependencyResolver<TDbContext> where TDbContext : DbContext
    {
        public static void RosolveCoreDependencies(IServiceCollection services, IConfiguration configuration)
        {
            // DbContext Mapping for IOC Container
            services.AddDbContext<DbContext, TDbContext>(opts =>
            {
                opts.UseSqlServer(
                    configuration["ConnectionStrings:Live"],
                    sqlOpts => sqlOpts.MigrationsHistoryTable("__EFMigrationsHistory").UseRelationalNulls()
                );
            });

            // Unit Of Work
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IUnitOfWorkManager, EFUnitOfWorkManager>();

            // Claims
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IClaimsPrincipalAccessor, HttpContextClaimsPrincipalAccessor>();

            // Object pooling
            services.AddObjectMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
