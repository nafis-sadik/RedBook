using Inventory.Data.Entities;
using RedBook.Core;
using RedBook.Core.Repositories;
using RedBook.Core.Security;

namespace Inventory.WebAPI.Configurations
{
    public static class DependencyResolver
    {
        public static void RosolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            CoreDependencyResolver<RedbookInventoryContext>.RosolveCoreDependencies(services, configuration);

            // Services
            services.AddScoped<IClaimsPrincipalAccessor, HttpContextClaimsPrincipalAccessor>();

            // Repositories
            services.AddScoped<IRepositoryBase<Bank>, RepositoryBase<Bank>>();
        }
    }
}
