using Identity.Data;
using Identity.Data.Entities;
using Identity.Domain.Abstraction;
using Identity.Domain.Implementation;
using Microsoft.EntityFrameworkCore;
using RedBook.Core;
using RedBook.Core.Repositories;
using RedBook.Core.Security;

namespace Identity.WebAPI.Configurations
{
    public static class DependencyResolver
    {
        public static void RosolveDependencies(this IServiceCollection services, Action<DbContextOptionsBuilder> dbOptionsBuilder)
        {
            CoreDependencyResolver<UserManagementSystemContext>.RosolveCoreDependencies(services, dbOptionsBuilder);
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
