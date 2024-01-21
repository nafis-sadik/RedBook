using Inventory.Data.Entities;
using Inventory.Domain.Abstraction;
using Inventory.Domain.Implementation;
using RedBook.Core;
using RedBook.Core.Security;

namespace Inventory.WebAPI.Configurations
{
    public static class DependencyResolver
    {
        public static void RosolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // DB Context & Other relevant mappings for Blume Core Library
            CoreDependencyResolver<RedbookInventoryContext>.RosolveCoreDependencies(services, configuration);

            // Services
            services.AddScoped<IClaimsPrincipalAccessor, HttpContextClaimsPrincipalAccessor>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISubcategoryService, SubcategoryService>();
        }
    }
}
