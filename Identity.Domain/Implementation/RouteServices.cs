using Identity.Data;
using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Http;
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
        private IRepositoryBase<Role> _roleRepo;
        private IRepositoryBase<Route> _routeRepo;
        private IRepositoryBase<Application> _appRepo;
        private IRepositoryBase<RoleRouteMapping> _roleMappingRepo;
        private IRepositoryBase<RouteType> _routeTypeRepo;

        public RouteServices(
            ILogger<RouteServices> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor)
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

        /// <summary>
        /// Return only the menu items allowed for requesting user that belongs to the request origin application
        /// </summary>
        public async Task<IEnumerable<RouteModel>> GetAllAppRoutes()
        {
            string? roleIds = User?.Claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role, StringComparison.InvariantCultureIgnoreCase))?.Value;
            int[] requesterRoleIds;
            if (!string.IsNullOrEmpty(roleIds))
            {
                string[] strArray = roleIds.Split(',');
                requesterRoleIds = Array.ConvertAll(strArray, int.Parse);
            }
            else
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _routeRepo = transaction.GetRepository<Route>();

                bool permissionFlag = false;
                foreach(int requesterRoleId in requesterRoleIds)
                {
                    var requesterRole = await _roleRepo.GetByIdAsync(requesterRoleId);
                    if(requesterRole != null && requesterRole.IsSystemAdmin)
                    {
                        permissionFlag = true;
                        break;
                    }
                }

                if (!permissionFlag) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                RouteModel[] requesterMenu = await _routeRepo
                    .UnTrackableQuery()
                    .Where(x => x.RouteId > 0)
                    .Select(x => new RouteModel
                    {
                        Id = x.RouteId,
                        RouteName = x.RouteName,
                        RouteValue = x.Route1,
                        Description = x.Description,
                    }).ToArrayAsync();

                if (requesterMenu == null)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

                return requesterMenu;
            }
        }

        public async Task<IEnumerable<RouteModel>> GetAppMenuRoutes()
        {
            List<RouteModel> routeList = new List<RouteModel>();
            string? roleIds = User?.Claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role, StringComparison.InvariantCultureIgnoreCase))?.Value;
            int[] requesterRoleIds;
            if (!string.IsNullOrEmpty(roleIds)){
                string[] strArray = roleIds.Split(',');
                requesterRoleIds = Array.ConvertAll(strArray, int.Parse);
            }
            else
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _routeRepo = transaction.GetRepository<Route>();
                _appRepo = transaction.GetRepository<Application>();
                _roleMappingRepo = transaction.GetRepository<RoleRouteMapping>();
                _routeTypeRepo = transaction.GetRepository<RouteType>();

                // Get all roles assigned to requester user
                List<Role> requesterRoles = new List<Role>();
                foreach (int roleId in requesterRoleIds)
                {
                    var role = await _roleRepo.GetByIdAsync(roleId);
                    if (role == null) { continue; }
                    requesterRoles.Add(role);
                }
                if (requesterRoles == null || requesterRoles.Count <= 0) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

                // Based on origin we shall 
                string origin = HttpContextAccessor.Request.Headers["Origin"].ToString();

                // Find request source applicaiton
                Application? requestSourceApp = await _appRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.ApplicationUrl == origin);
                if (requestSourceApp == null) throw new ArgumentException("Request from invalid application");

                foreach (Role requesterRole in requesterRoles)
                {
                    // If the user is a sys admin user, return all available routes
                    if (requesterRole.IsSystemAdmin){
                        routeList = await _routeRepo
                        .UnTrackableQuery()
                        .Where(x => x.ApplicationId > 0)
                        .Select(x => new RouteModel
                        {
                            Id = x.RouteId,
                            RouteValue = x.Route1,
                            RouteName = x.RouteName,
                            Description = x.Description,
                            ParentRouteId = x.ParentRouteId
                        }).ToListAsync();

                        break;
                    } else if (requesterRole.IsAdmin) {
                        var adminRoute = await _routeTypeRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.RouteTypeName == RouteTypeConsts.AdminRoute.RouteTypeName);
                        var genericRoute = await _routeTypeRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.RouteTypeName == RouteTypeConsts.GenericRoute.RouteTypeName);

                        routeList.AddRange(await _routeRepo.UnTrackableQuery()
                            .Where(x => x.ApplicationId == requestSourceApp.ApplicationId && (x.RouteTypesId == adminRoute.RouteTypeId || x.RouteTypesId == genericRoute.RouteTypeId))
                            .Select(x => new RouteModel
                            {
                                Id = x.RouteId,
                                RouteValue = x.Route1,
                                RouteName = x.RouteName,
                                Description = x.Description,
                                ParentRouteId = x.ParentRouteId
                            }).ToListAsync());

                    } else {
                        routeList.AddRange(await _roleMappingRepo
                        .UnTrackableQuery()
                        .Where(x => x.RoleId == requesterRole.RoleId && x.Route.ApplicationId == requestSourceApp.ApplicationId)
                        .Select(x => new RouteModel
                        {
                            Id = x.RouteId,
                            RouteValue = x.Route.Route1,
                            RouteName = x.Route.RouteName,
                            Description = x.Route.Description,
                            ParentRouteId = x.Route.ParentRouteId
                        }).ToListAsync());
                    }
                }

                if (routeList == null)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

                return routeList;
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
                            ParentRouteId = x.ParentRouteId,
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
                            (x.RouteName.ToLower().Contains(pagedRoutes.SearchString.ToLower())
                            || x.Route1.ToLower().Contains(pagedRoutes.SearchString.ToLower())
                            || x.Description.ToLower().Contains(pagedRoutes.SearchString.ToLower())
                            || x.Application.ApplicationName.ToLower().Contains(pagedRoutes.SearchString.ToLower())))
                        .Skip(pagedRoutes.Skip)
                        .Take(pagedRoutes.PageSize)
                        .Select(x => new RouteModel
                        {
                            Id = x.RouteId,
                            ApplicationId = x.ApplicationId,
                            Description = x.Description,
                            RouteName = x.RouteName,
                            RouteValue = x.Route1,
                            ParentRouteId = x.ParentRouteId,
                            ApplicationName = x.Application.ApplicationName
                        }).ToListAsync();

                    pagedRoutes.TotalItems = await _routeRepo.UnTrackableQuery()
                        .Where(x =>
                            x.RouteId > 0 &&
                            (x.RouteName.ToLower().Contains(pagedRoutes.SearchString.ToLower())
                            || x.Route1.ToLower().Contains(pagedRoutes.SearchString.ToLower())
                            || x.Description.ToLower().Contains(pagedRoutes.SearchString.ToLower())
                            || x.Application.ApplicationName.ToLower().Contains(pagedRoutes.SearchString.ToLower())))
                        .CountAsync();                
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

                routeEntity.RouteName = routeModel.RouteName;
                routeEntity.Route1 = routeModel.RouteValue;
                routeEntity.ApplicationId = routeModel.ApplicationId;
                routeEntity.ParentRouteId = routeModel.ParentRouteId;
                routeEntity.Description = routeModel.Description;

                routeEntity = _routeRepo.Update(routeEntity);

                await transaction.SaveChangesAsync();
            }
            var data = Mapper.Map<RouteModel>(routeEntity);
            return Mapper.Map<RouteModel>(routeEntity);
        }
    }
}
