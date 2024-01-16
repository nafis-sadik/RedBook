using Google.Protobuf.WellKnownTypes;
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
using System.Collections.Generic;

namespace Identity.Domain.Implementation
{
    public class RouteServices : ServiceBase, IRouteServices
    {
        private IRepositoryBase<Role> _roleRepo;
        private IRepositoryBase<Route> _routeRepo;
        private IRepositoryBase<Application> _appRepo;
        private IRepositoryBase<UserRole> _userRoleMappingRepo;
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
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _routeRepo = transaction.GetRepository<Route>();
                _roleRepo = transaction.GetRepository<Role>();
                _roleMappingRepo = transaction.GetRepository<RoleRouteMapping>();

                if (!await this.HasSystemAdminPriviledge(_roleRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                // Add the route
                Route routeEntity = await _routeRepo.InsertAsync(Mapper.Map<Route>(routeModel));

                await transaction.SaveChangesAsync();

                // Allow system admin users to access the route
                Role[]? sysAdminRoles = await _roleRepo.UnTrackableQuery().Where(x => x.IsSystemAdmin == true).ToArrayAsync();

                foreach(var role in sysAdminRoles)
                {
                    await _roleMappingRepo.InsertAsync(new RoleRouteMapping
                    {
                        RoleId = role.RoleId,
                        RouteId = routeEntity.RouteId,
                    });
                }

                await transaction.SaveChangesAsync();

                return Mapper.Map<RouteModel>(routeEntity);
            }
        }

        public async Task DeleteRoute(int routeId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _routeRepo = transaction.GetRepository<Route>();

                if (!await this.HasSystemAdminPriviledge(_roleRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                await _routeRepo.DeleteAsync(routeId);
                await transaction.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Return only the menu items allowed for requesting user that belongs to the request origin application
        /// </summary>
        public async Task<IEnumerable<RouteModel>> GetAllAppRoutes(int appId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _routeRepo = transaction.GetRepository<Route>();
                _userRoleMappingRepo = transaction.GetRepository<UserRole>();

                var query = _routeRepo.UnTrackableQuery();

                if (await this.HasSystemAdminPriviledge(_roleRepo)) {
                    query = query.Where(x => x.ApplicationId == appId);
                } else {
                    if (_userRoleMappingRepo.TrackableQuery().Where(x => x.UserId == User.UserId && x.Role.IsAdmin).Any())
                    {
                        query = query.Where(x => x.ApplicationId == appId && x.RouteId != RouteTypeConsts.SysAdminRoute.RouteTypeId && x.RouteId != RouteTypeConsts.RetailerRoute.RouteTypeId);
                    }
                    else if (await this.HasRetailerPriviledge(_roleRepo))
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

        public async Task<IEnumerable<RouteModel>> GetAppMenuRoutes(int appId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _routeRepo = transaction.GetRepository<Route>();
                _appRepo = transaction.GetRepository<Application>();

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
                if (requestSourceApp.ApplicationId != appId) throw new ArgumentException("Request from invalid application");

                IQueryable<Route> query = _routeRepo.UnTrackableQuery();
                foreach (Role requesterRole in requesterRoles)
                {
                    // If the user is a sys admin user, return all available routes
                    if (requesterRole.IsSystemAdmin){
                        query = query.Where(x => x.ApplicationId == appId);
                        break;
                    } else if (requesterRole.IsAdmin) {
                        query = query.Where(x => x.ApplicationId == requestSourceApp.ApplicationId
                            && (x.RouteTypesId == RouteTypeConsts.AdminRoute.RouteTypeId || x.RouteTypesId == RouteTypeConsts.GenericRoute.RouteTypeId)).Distinct();
                    } else {
                        query = query.Where(x => x.ApplicationId == requestSourceApp.ApplicationId && x.RouteTypesId == RouteTypeConsts.GenericRoute.RouteTypeId).Distinct();
                    }
                }

                return await query.Select(x => new RouteModel
                {
                    RouteId = x.RouteId,
                    RouteValue = x.Route1,
                    RouteName = x.RouteName,
                    Description = x.Description,
                    ParentRouteId = x.ParentRouteId
                }).ToListAsync();
            }
        }

        public async Task<PagedModel<RouteModel>> GetPagedRoutes(PagedModel<RouteModel> pagedRoutes)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _routeRepo = transaction.GetRepository<Route>();

                if (! await this.HasSystemAdminPriviledge(_roleRepo))
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
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _routeRepo = transaction.GetRepository<Route>();
                Route? routeEntity = await _routeRepo.GetAsync(routeId);

                if (routeEntity != null)
                    return Mapper.Map<RouteModel>(routeEntity);
                else
                    return null;
            }
        }

        public async Task<IEnumerable<RouteModel>> GetRoutesByRoleId(int roleId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleMappingRepo = transaction.GetRepository<RoleRouteMapping>();

                var data = await _roleMappingRepo.UnTrackableQuery()
                    .Where(x => x.RoleId == roleId && (x.Route.RouteTypesId == RouteTypeConsts.GenericRoute.RouteTypeId || x.Route.RouteTypesId == RouteTypeConsts.AdminRoute.RouteTypeId))
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
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _routeRepo = transaction.GetRepository<Route>();
                _roleRepo = transaction.GetRepository<Role>();

                if (!await this.HasSystemAdminPriviledge(_roleRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

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

                await transaction.SaveChangesAsync();
                return Mapper.Map<RouteModel>(routeEntity);
            }
        }
    }
}
