using Inventory.Data;
using Inventory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using RedBook.Core;
using RedBook.Core.Security;

namespace Inventory.WebAPI.Configurations
{
    public static class DependencyResolver
    {
        public static void RosolveDependencies(this IServiceCollection services)
        {
            CoreDependencyResolver<RedbookInventoryContext>.RosolveCoreDependencies(services);

            // Services
            services.AddScoped<IClaimsPrincipalAccessor, HttpContextClaimsPrincipalAccessor>();

            // Repositories
            //services.AddScoped<IRepositoryBase<User>, RepositoryBase<User>>();
        }
    }
}
