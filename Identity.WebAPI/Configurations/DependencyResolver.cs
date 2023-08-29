using Identity.Data.Entities;
using Identity.Domain.Abstraction;
using Identity.Domain.Implementation;
using Microsoft.EntityFrameworkCore;
using RedBook.Core;
using RedBook.Core.Repositories;

namespace Identity.WebAPI.Configurations
{
    /// <summary>
    /// Extension methods for resolving dependencies in Constructor DI using IOC Container
    /// </summary>
    public static class DependencyResolver
    {
        /// <summary>
        /// Registers Interfaces with corresponding implementation Classes for IOC Contaienr.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        public static void RosolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // DB Context & Other relevant mappings for Blume Core Library
            CoreDependencyResolver<RedbookIdentityContext>.RosolveCoreDependencies(services, configuration);

            // Application Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<IRouteServices, RouteServices>();

            // Application Repositories
            services.AddScoped<IRepositoryBase<User>, RepositoryBase<User>>();
            services.AddScoped<IRepositoryBase<Role>, RepositoryBase<Role>>();
        }
    }
}
