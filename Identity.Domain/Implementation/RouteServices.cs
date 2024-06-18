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
using System.Linq;

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

                var query = _routeRepo.UnTrackableQuery().Where(x => x.ApplicationId == appId);

                if (!await this.HasSystemAdminPriviledge(_userRoleMappingRepo))
                {
                    if (_userRoleMappingRepo.UnTrackableQuery().Where(x => x.UserId == User.UserId && x.Role.IsOwner).Any())
                        query = query.Where(x => x.RouteId != RouteTypeConsts.OrganizationOwner.RouteTypeId);
                    else if (await this.HasRetailerPriviledge(_userRoleMappingRepo))
                        query = query.Where(x => x.RouteId == RouteTypeConsts.RetailerRoute.RouteTypeId);
                    else
                        throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);
                } 

                var data = await query.Select(x => new RouteModel
                {
                    RouteId = x.RouteId,
                    RouteName = x.RouteName,
                    RouteValue = x.RoutePath,
                    Description = x.Description,
                    ParentRouteId = x.ParentRouteId,
                }).ToArrayAsync();

                return data;
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
                var _roleRouteMappingRepo = factory.GetRepository<RoleRouteMapping>();

                // Get all roles assigned to requester user

                //var data = from userRoleMapping in _userRoleMappingRepo.UnTrackableQuery()
                //           join roleRouteMapping in _roleRouteMappingRepo.UnTrackableQuery()
                //           on userRoleMapping.RoleId equals roleRouteMapping.RoleId
                //           where userRoleMapping.UserId == User.UserId && userRoleMapping.Role.Application.ApplicationUrl == User.RequestOrigin
                //           select roleRouteMapping;

                //var og = await data.ToListAsync();

                List<Role> requesterRoles = await _userRoleMappingRepo.UnTrackableQuery()
                    .Where(x => x.UserId == User.UserId && x.Role.Application.ApplicationUrl == User.RequestOrigin)
                    .Select(x => new Role
                    {
                        RoleId = x.RoleId,
                        ApplicationId = x.Role.ApplicationId,
                        IsSystemAdmin = x.Role.IsSystemAdmin,
                        IsAdmin = x.Role.IsAdmin,
                        IsOwner = x.Role.IsOwner,
                        IsRetailer = x.Role.IsRetailer,
                        OrganizationId = x.Role.OrganizationId
                    })
                    .ToListAsync();

                int appId = await _appRepo.UnTrackableQuery().Where(x => x.ApplicationUrl == User.RequestOrigin).Select(x => x.ApplicationId).FirstOrDefaultAsync();

                var query = _routeRepo.UnTrackableQuery().Where(x => x.ApplicationId == appId && x.IsMenuRoute);

                // If the user is a sys admin user, return all available routes
                if (requesterRoles.Any(x => x.IsSystemAdmin))
                {
                    query = query.Where(x => x.RouteId > 0);
                }
                else if(requesterRoles.Any(x => x.IsOwner))
                {
                    query = query.Where(x => x.RouteTypeId == RouteTypeConsts.OrganizationOwner.RouteTypeId);
                }
                else
                {
                    foreach (Role requesterRole in requesterRoles)
                    {
                        if (requesterRole.IsAdmin)
                        {
                            query = query.Where(x => (x.RouteTypeId == RouteTypeConsts.AdminRoute.RouteTypeId || x.RouteTypeId == RouteTypeConsts.GenericRoute.RouteTypeId)).Distinct();
                        }
                        else
                        {
                            query = query.Where(x => x.ApplicationId == requesterRole.ApplicationId && x.RouteTypeId == RouteTypeConsts.GenericRoute.RouteTypeId).Distinct();
                        }
                    }
                }

                return await query.Select(x => new RouteModel
                {
                    RouteId = x.RouteId,
                    RouteValue = x.RoutePath,
                    RouteName = x.RouteName,
                    Description = x.Description,
                    ParentRouteId = x.ParentRouteId
                }).ToListAsync();
            }
        }

        public async Task<PagedModel<RouteModel>> GetPagedRoutes(PagedModel<RouteModel> pagedRoutes)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleRepo = factory.GetRepository<Role>();
                _routeRepo = factory.GetRepository<Route>();
                var _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                if (!await this.HasSystemAdminPriviledge(_userRoleRepo))
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                if (string.IsNullOrEmpty(pagedRoutes.SearchString))
                {
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
                            RouteValue = x.RoutePath,
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
                            || x.RoutePath.ToLower().Contains(pagedRoutes.SearchString.ToLower())
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
                            RouteValue = x.RoutePath,
                            ParentRouteId = x.ParentRouteId,
                            ApplicationName = x.Application.ApplicationName
                        })
                        .ToListAsync();

                    pagedRoutes.TotalItems = await _routeRepo.UnTrackableQuery()
                        .Where(x =>
                            x.RouteId > 0 &&
                            (x.RouteName.ToLower().Contains(pagedRoutes.SearchString.ToLower())
                            || x.RoutePath.ToLower().Contains(pagedRoutes.SearchString.ToLower())
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
                _appRepo = factory.GetRepository<Application>();
                _routeRepo = factory.GetRepository<Route>();
                _roleMappingRepo = factory.GetRepository<RoleRouteMapping>();

                int appId = await _appRepo.UnTrackableQuery()
                    .Where(x => x.ApplicationUrl == User.RequestOrigin)
                    .Select(x => x.ApplicationId)
                    .FirstOrDefaultAsync();

                if (roleId != RoleConstants.SystemAdmin.RoleId)
                {
                    var query = _roleMappingRepo.UnTrackableQuery().Where(x => x.RouteId > 0);

                    if (roleId == RoleConstants.OwnerAdmin.RoleId || roleId == RoleConstants.RedbookAdmin.RoleId)
                    {
                        query = query.Where(x => x.Role.ApplicationId == appId);
                    }
                    else
                    {
                        query = query.Where(x => x.RoleId == roleId);
                    }

                    return await query
                        .Select(x => new RouteModel
                        {
                            RouteId = x.Route.RouteId,
                            RouteName = x.Route.RouteName
                        }).ToListAsync();
                }
                else
                {
                    var data = await _routeRepo.UnTrackableQuery()
                        .Where(x => x.IsMenuRoute && x.ApplicationId == appId)
                        .Select(x => new RouteModel
                        {
                            RouteId = x.RouteId,
                            RouteName = x.RouteName
                        }).ToListAsync();

                    return data;
                }
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
