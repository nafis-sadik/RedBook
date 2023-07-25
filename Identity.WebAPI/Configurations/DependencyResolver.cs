using Identity.Data.Entities;
using Identity.Domain.Abstraction;
using Identity.Domain.Implementation;
using RedBook.Core;
using RedBook.Core.Repositories;
using RedBook.Core.Security;

namespace Identity.WebAPI.Configurations
{
    public static class DependencyResolver
    {
        public static void RosolveDependencies(this IServiceCollection services)
        {
            CoreDependencyResolver<UserManagementSystemContext>.RosolveCoreDependencies(services);
            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IClaimsPrincipalAccessor, HttpContextClaimsPrincipalAccessor>();

            // Repositories
            services.AddScoped<IRepositoryBase<User>, RepositoryBase<User>>();
            services.AddScoped<IRepositoryBase<Role>, RepositoryBase<Role>>();
        }
    }
}
