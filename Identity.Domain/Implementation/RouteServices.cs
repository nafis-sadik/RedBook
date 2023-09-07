using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Domain;
using RedBook.Core.Models;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using System.Security.Claims;

namespace Identity.Domain.Implementation
{
    public class RouteServices : ServiceBase, IRouteServices
    {
        private IRepositoryBase<Route> _routeRepo;
        private IRepositoryBase<Role> _roleRepo;
        private IRepositoryBase<RoleRouteMapping> _roleMappingRepo;
        public RouteServices(
            ILogger<RouteServices> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        { }

        public async Task<RouteModel> AddRoute(RouteModel routeModel)
        {
            string? requesterRoleIdStr = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;
            if (string.IsNullOrEmpty(requesterRoleIdStr) || !int.TryParse(requesterRoleIdStr, out int requesterRoleId))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            Route routeEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _routeRepo = transaction.GetRepository<Route>();
                _roleRepo = transaction.GetRepository<Role>();
                _roleMappingRepo = transaction.GetRepository<RoleRouteMapping>();

                Role? requesterRoleEntity = await _roleRepo.GetByIdAsync(requesterRoleId);
                if (requesterRoleEntity == null)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

                // Only System Admin should have access to this api
                if (!requesterRoleEntity.IsSystemAdmin)
                    throw new ArgumentException($"Only system admin user have access to perform this operation");

                routeEntity = Mapper.Map<Route>(routeModel);

                routeEntity = await _routeRepo.InsertAsync(routeEntity);

                await transaction.SaveChangesAsync();

                var sysAdminRoles = await _roleRepo.UnTrackableQuery().Where(x => x.IsSystemAdmin == true).ToListAsync();

                foreach(var role in sysAdminRoles)
                {
                    await _roleMappingRepo.InsertAsync(new RoleRouteMapping
                    {
                        RoleId = role.RoleId,
                        RouteId = routeEntity.RouteId,
                    });
                }

                await transaction.SaveChangesAsync();
            }

            return Mapper.Map<RouteModel>(routeEntity);
        }

        public async Task DeleteRoute(int routeId)
        {
            string? requesterRoleIdStr = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;
            if (string.IsNullOrEmpty(requesterRoleIdStr) || !int.TryParse(requesterRoleIdStr, out int requesterRoleId))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            using (var transaction = UnitOfWorkManager.Begin())
            {
                _routeRepo = transaction.GetRepository<Route>();
                _roleRepo = transaction.GetRepository<Role>();

                Role? requesterRoleEntity = await _roleRepo.GetByIdAsync(requesterRoleId);
                if (requesterRoleEntity == null)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

                // Only System Admin should have access to this api
                if (!requesterRoleEntity.IsSystemAdmin)
                    throw new ArgumentException($"Only system admin user have access to perform this operation");

                await _routeRepo.DeleteAsync(routeId);
                await transaction.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<RouteModel>> GetAllRoutes()
        {
            int requesterRoleId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role, StringComparison.InvariantCultureIgnoreCase))?.Value);

            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleMappingRepo = transaction.GetRepository<RoleRouteMapping>();

                List<RouteModel> requester = await _roleMappingRepo
                    .UnTrackableQuery()
                    .Where(x => x.RoleId == requesterRoleId)
                    .Select(x => new RouteModel {
                        Id = x.RouteId,
                        RouteName = x.Route.RouteName
                    }).ToListAsync();

                if (requester == null)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

                return requester;
            }
        }

        public async Task<PagedModel<RouteModel>> GetPagedRoutes(PagedModel<RouteModel> pagedRoutes)
        {
            string? requesterRoleIdStr = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;
            if (string.IsNullOrEmpty(requesterRoleIdStr) || !int.TryParse(requesterRoleIdStr, out int requesterRoleId))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _routeRepo = transaction.GetRepository<Route>();

                Role? requesterRoleModel = await _roleRepo.GetByIdAsync(requesterRoleId);
                if (requesterRoleModel == null || !requesterRoleModel.IsSystemAdmin)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                if (string.IsNullOrEmpty(pagedRoutes.SearchString)) {
                    pagedRoutes.SourceData = await _routeRepo
                        .UnTrackableQuery()
                        .Where(x => x.RouteId > 0)
                        .Skip(pagedRoutes.Skip)
                        .Take(pagedRoutes.PageSize)
                        .Select(x => new RouteModel
                        {
                            Id = x.RouteId,
                            ApplicationId = x.ApplicationId,
                            Description = x.Description,
                            RouteName = x.RouteName,
                            RouteValue = x.Route1,
                            ApplicationName = x.Application.ApplicationName
                        }).ToListAsync();

                    pagedRoutes.TotalItems = await _routeRepo.UnTrackableQuery()
                        .Where(x => x.RouteId > 0).CountAsync();
                }
                else
                {
                    pagedRoutes.SourceData = await _routeRepo
                        .UnTrackableQuery()
                        .Where(x =>
                            x.RouteId > 0 &&
                            (x.RouteName.Contains(pagedRoutes.SearchString)
                            || x.Route1.Contains(pagedRoutes.SearchString)))
                        .Skip(pagedRoutes.Skip)
                        .Take(pagedRoutes.PageSize)
                        .Select(x => new RouteModel
                        {
                            Id = x.RouteId,
                            ApplicationId = x.ApplicationId,
                            Description = x.Description,
                            RouteName = x.RouteName,
                            RouteValue = x.Route1,
                            ApplicationName = x.Application.ApplicationName
                        }).ToListAsync();

                    pagedRoutes.TotalItems = await _routeRepo.UnTrackableQuery()
                        .Where(x => x.RouteId > 0 &&
                                (x.RouteName.Contains(pagedRoutes.SearchString)
                                || x.Route1.Contains(pagedRoutes.SearchString))).CountAsync();
                
                }

            }

            return pagedRoutes;
        }

        public async Task<RouteModel?> GetRoute(int routeId)
        {
            Route? routeEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _routeRepo = transaction.GetRepository<Route>();
                routeEntity = await _routeRepo.GetByIdAsync(routeId);
            }
            if (routeEntity != null)
                return Mapper.Map<RouteModel>(routeEntity);
            else
                return null;
        }

        public async Task<RouteModel> UpdateRoute(RouteModel routeModel)
        {
            string? requesterRoleIdStr = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;
            if (string.IsNullOrEmpty(requesterRoleIdStr) || !int.TryParse(requesterRoleIdStr, out int requesterRoleId))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            Route? routeEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _routeRepo = transaction.GetRepository<Route>();
                _roleRepo = transaction.GetRepository<Role>();

                Role? requesterRoleEntity = await _roleRepo.GetByIdAsync(requesterRoleId);
                if (requesterRoleEntity == null)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

                // Only System Admin should have access to this api
                if (!requesterRoleEntity.IsSystemAdmin)
                    throw new ArgumentException($"Only system admin user have access to perform this operation");

                routeEntity = await _routeRepo.GetByIdAsync(routeModel.Id);
                if (routeEntity == null)
                    throw new ArgumentException($"Not route with identifier {routeModel.Id} was found");

                routeEntity = Mapper.Map(routeModel, routeEntity);
                routeEntity = _routeRepo.Update(routeEntity);

                await transaction.SaveChangesAsync();
            }

            return Mapper.Map<RouteModel>(routeEntity);
        }
    }
}
