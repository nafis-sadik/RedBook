using Identity.Data.Entities;
using Identity.Domain.Abstraction;
using Identity.Domain.Implementation;
using RedBook.Core;
using RedBook.Core.EntityFramework;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Identity.WebAPI.Configurations
{
    public static class DependencyResolver
    {
        public static void RosolveDependencies(this IServiceCollection services)
        {
            CoreDependencyResolver<RedbookIdentityContext>.RosolveCoreDependencies(services);
            // Services
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IUnitOfWorkManager, EFUnitOfWorkManager>();
            services.AddScoped<IClaimsPrincipalAccessor, HttpContextClaimsPrincipalAccessor>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IApplicationService, ApplicationService>();

            // Repositories
            services.AddScoped<IRepositoryBase<User>, RepositoryBase<User>>();
            services.AddScoped<IRepositoryBase<Role>, RepositoryBase<Role>>();
        }
    }
}
