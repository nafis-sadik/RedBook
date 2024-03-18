using Identity.Data.CommonConstant;
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

namespace Identity.Domain.Implementation
{
    public class RouteServices : ServiceBase, IRouteServices
    {
        private IRepositoryBase<Role> _roleRepo;
        private IRepositoryBase<Route> _routeRepo;
        private IRepositoryBase<Application> _appRepo;
        private IRepositoryBase<UserRoleMapping> _userRoleMappingRepo;
        private IRepositoryBase<RoleRouteMapping> _roleMappingRepo;
        private IRepositoryBase<RouteType> _routeTypeRepo; 

        public RouteServices(
            ILogger<RouteServices> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor)
        { }
        
        public async Task<RouteModel> AddRoute(RouteModel routeModel)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _routeRepo = factory.GetRepository<Route>();
                _roleRepo = factory.GetRepository<Role>();
                _userRoleMappingRepo = factory.GetRepository<UserRoleMapping>();
                _roleMappingRepo = factory.GetRepository<RoleRouteMapping>();

                if (!await this.HasSystemAdminPriviledge(_userRoleMappingRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                // Add the route
                Route routeEntity = await _routeRepo.InsertAsync(Mapper.Map<Route>(routeModel));

                await factory.SaveChangesAsync();

                return Mapper.Map<RouteModel>(routeEntity);
            }
        }

        public async Task DeleteRoute(int routeId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleRepo = factory.GetRepository<Role>();
                _routeRepo = factory.GetRepository<Route>();
                _userRoleMappingRepo = factory.GetRepository<UserRoleMapping>();

                if (!await this.HasSystemAdminPriviledge(_userRoleMappingRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                await _routeRepo.DeleteAsync(routeId);
                await factory.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Return only the menu items allowed for requesting user that belongs to the request origin application
        /// </summary>
        public async Task<IEnumerable<RouteModel>> GetAllAppRoutes(int appId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleRepo = factory.GetRepository<Role>();
                _routeRepo = factory.GetRepository<Route>();
                _userRoleMappingRepo = factory.GetRepository<UserRoleMapping>();

                var query = _routeRepo.UnTrackableQuery();

                if (await this.HasSystemAdminPriviledge(_userRoleMappingRepo)) {
                    query = query.Where(x => x.ApplicationId == appId);
                } else {
                    if (_userRoleMappingRepo.TrackableQuery().Where(x => x.UserId == User.UserId && x.Role.IsAdmin == true).Any())
                    {
                        query = query.Where(x => x.ApplicationId == appId && x.RouteId != RouteTypeConsts.SysAdminRoute.RouteTypeId && x.RouteId != RouteTypeConsts.RetailerRoute.RouteTypeId);
                    }
                    else if (await this.HasRetailerPriviledge(_userRoleMappingRepo))
                    {
                        query = query.Where(x => x.ApplicationId == appId && x.RouteId == RouteTypeConsts.RetailerRoute.RouteTypeId);
                    }
                    else
                    {
                        throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);
                    }
                }

                return await query.Select(x => new RouteModel
                    {
                        RouteId = x.RouteId,
                        RouteName = x.RouteName,
                        RouteValue = x.Route1,
                        Description = x.Description,
                        ParentRouteId = x.ParentRouteId,
                    }).ToArrayAsync();
            }
        }

        public async Task<IEnumerable<RouteModel>> GetAppMenuRoutes()
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleRepo = factory.GetRepository<Role>();
                _routeRepo = factory.GetRepository<Route>();
                _appRepo = factory.GetRepository<Application>();
                _userRoleMappingRepo = factory.GetRepository<UserRoleMapping>();

                // Get all roles assigned to requester user
                List<Role> requesterRoles = new List<Role>();
                foreach (int roleId in User.RoleIds)
                {
                    Role? role = await _roleRepo.GetAsync(roleId);
                    if (role == null) { continue; }
                    requesterRoles.Add(role);
                }

                // Based on origin we shall 
                string? origin = HttpContextAccessor?.Request.Headers["Origin"].ToString();

                // Find request source applicaiton
                Application? requestSourceApp = await _appRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.ApplicationUrl == origin);
                if (requestSourceApp == null) throw new ArgumentException("Request from invalid application");
                
                var query = _routeRepo.UnTrackableQuery();
                foreach (Role requesterRole in requesterRoles)
                {
                    // If the user is a sys admin user, return all available routes
                    if (requesterRole.IsSystemAdmin == true) {
                        query = query.Where(x => x.ApplicationId == requestSourceApp.ApplicationId);
                        break;
                    } else if (requesterRole.IsAdmin == true) {
                        query = query.Where(x => x.ApplicationId == requestSourceApp.ApplicationId
                            && (x.RouteTypeId == RouteTypeConsts.AdminRoute.RouteTypeId || x.RouteTypeId == RouteTypeConsts.GenericRoute.RouteTypeId)).Distinct();
                    } else {
                        query = query.Where(x => x.ApplicationId == requestSourceApp.ApplicationId && x.RouteTypeId == RouteTypeConsts.GenericRoute.RouteTypeId).Distinct();
                    }
                }

                try
                {
                    return await query.Select(x => new RouteModel
                    {
                        RouteId = x.RouteId,
                        RouteValue = x.Route1,
                        RouteName = x.RouteName,
                        Description = x.Description,
                        ParentRouteId = x.ParentRouteId
                    }).ToListAsync();
                }catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }

        public async Task<PagedModel<RouteModel>> GetPagedRoutes(PagedModel<RouteModel> pagedRoutes)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleRepo = factory.GetRepository<Role>();
                var _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                if (! await this.HasSystemAdminPriviledge(_userRoleRepo))
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                if (string.IsNullOrEmpty(pagedRoutes.SearchString)) {
                    pagedRoutes.SourceData = await _routeRepo
                        .UnTrackableQuery()
                        .Where(x => x.RouteId > 0)
                        .Skip(pagedRoutes.Skip)
                        .Take(pagedRoutes.PageLength)
                        .Select(x => new RouteModel
                        {
                            RouteId = x.RouteId,
                            ApplicationId = x.ApplicationId,
                            Description = x.Description,
                            RouteName = x.RouteName,
                            RouteValue = x.Route1,
                            ParentRouteId = x.ParentRouteId,
                            ApplicationName = x.Application.ApplicationName
                        })
                        .ToListAsync();

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
                        .Take(pagedRoutes.PageLength)
                        .Select(x => new RouteModel
                        {
                            RouteId = x.RouteId,
                            ApplicationId = x.ApplicationId,
                            Description = x.Description,
                            RouteName = x.RouteName,
                            RouteValue = x.Route1,
                            ParentRouteId = x.ParentRouteId,
                            ApplicationName = x.Application.ApplicationName
                        })
                        .ToListAsync();

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
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _routeRepo = factory.GetRepository<Route>();
                Route? routeEntity = await _routeRepo.GetAsync(routeId);

                if (routeEntity != null)
                    return Mapper.Map<RouteModel>(routeEntity);
                else
                    return null;
            }
        }

        public async Task<IEnumerable<RouteModel>> GetRoutesByRoleId(int roleId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleMappingRepo = factory.GetRepository<RoleRouteMapping>();
                _userRoleMappingRepo = factory.GetRepository<UserRoleMapping>();

                var data = await _roleMappingRepo.UnTrackableQuery()
                    .Where(x => x.RoleId == roleId && (x.Route.RouteTypeId == RouteTypeConsts.GenericRoute.RouteTypeId || x.Route.RouteTypeId == RouteTypeConsts.AdminRoute.RouteTypeId))
                    .Select(x => new RouteModel
                    {
                        RouteId = x.Route.RouteId,
                        RouteName = x.Route.RouteName
                    }).ToListAsync();

                return data;
            }
        }

        public async Task<RouteModel> UpdateRoute(RouteModel routeModel)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _routeRepo = factory.GetRepository<Route>();
                _roleRepo = factory.GetRepository<Role>();
                _userRoleMappingRepo = factory.GetRepository<UserRoleMapping>();

                if (!await this.HasSystemAdminPriviledge(_userRoleMappingRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                Route? routeEntity = await _routeRepo.GetAsync(routeModel.RouteId);
                if (routeEntity == null)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);

                //routeEntity.RouteName = routeModel.RouteName;
                //routeEntity.Route1 = routeModel.RouteValue;
                //routeEntity.ApplicationId = routeModel.ApplicationId;
                //routeEntity.ParentRouteId = routeModel.ParentRouteId;
                //routeEntity.Description = routeModel.Description;

                Mapper.Map(routeModel, routeEntity);

                routeEntity = _routeRepo.Update(routeEntity);

                await factory.SaveChangesAsync();
                return Mapper.Map<RouteModel>(routeEntity);
            }
        }
    }
}
