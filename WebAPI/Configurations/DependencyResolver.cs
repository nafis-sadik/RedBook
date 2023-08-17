using Inventory.Data;
using RedBook.Core;
using RedBook.Core.Security;

namespace Inventory.WebAPI.Configurations
{
    public static class DependencyResolver
    {
        public static void RosolveDependencies(this IServiceCollection services)
        {
            CoreDependencyResolver<RedBookInventoryContext>.RosolveCoreDependencies(services);

            // Services
            services.AddScoped<IClaimsPrincipalAccessor, HttpContextClaimsPrincipalAccessor>();

            // Repositories
            //services.AddScoped<IRepositoryBase<User>, RepositoryBase<User>>();
        }
    }
}
