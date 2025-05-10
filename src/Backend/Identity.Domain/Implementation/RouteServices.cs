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
                Route routeEntity = Mapper.Map<Route>(routeModel);
                routeEntity = await _routeRepo.InsertAsync(routeEntity);

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
        public async Task<IEnumerable<RouteModel>> GetAppRoutes()
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleRepo = factory.GetRepository<Role>();
                _routeRepo = factory.GetRepository<Route>();
                _userRoleMappingRepo = factory.GetRepository<UserRoleMapping>();

                var query = _routeRepo.UnTrackableQuery().Where(x => x.Application.ApplicationUrl == User.RequestOrigin);

                //if (!await this.HasSystemAdminPriviledge(_userRoleMappingRepo))
                //{
                //    if (await this.HasOwnerPriviledge(_userRoleMappingRepo))
                //        query = query.Where(x => x.RouteId != RouteTypeConsts.OrganizationOwner.RouteTypeId);
                //    else if (await this.HasRetailerPriviledge(_userRoleMappingRepo))
                //        query = query.Where(x => x.RouteId == RouteTypeConsts.RetailerRoute.RouteTypeId);
                //    else
                //        throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);
                //}

                var data = await query.Select(x => new RouteModel
                {
                    RouteId = x.RouteId,
                    RouteName = x.RouteName,
                    RouteValue = x.RoutePath,
                    Description = x.Description,
                    ParentRouteId = x.ParentRouteId
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

                var userMenuRoutes = await _userRoleMappingRepo.UnTrackableQuery()
                    .Where(userRole => userRole.UserId == User.UserId && userRole.Role.Application.ApplicationUrl == User.RequestOrigin)
                    .Select(userRole => userRole.Role.RoleRouteMappings.Where(roleRoute => roleRoute.Route.IsMenuRoute)
                        .Select(roleRoutes => new RouteModel
                        {
                            RouteId = roleRoutes.Route.RouteId,
                            RouteName = roleRoutes.Route.RouteName,
                            RouteValue = roleRoutes.Route.RoutePath,
                            ParentRouteId = roleRoutes.Route.ParentRouteId,
                            Description = roleRoutes.Route.Description,
                        }))
                    .ToListAsync();

                var routeQuery = _routeRepo.UnTrackableQuery()
                    .Where(x => x.Application.ApplicationUrl == User.RequestOrigin && x.IsMenuRoute);

                List<RouteModel> genericRoutes = new List<RouteModel>();
                foreach (IEnumerable<RouteModel> menuList in userMenuRoutes) {
                    genericRoutes.AddRange(menuList);
                }

                genericRoutes.Distinct();

                IEnumerable<int?> parentRoutesId = genericRoutes.Where(x => x.ParentRouteId != null).Select(x => x.ParentRouteId).ToList().Distinct();
                foreach (int parentId in parentRoutesId)
                {
                    if (!genericRoutes.Any(x => x.RouteId == parentId))
                    {
                        RouteModel parentRoute = await _routeRepo.UnTrackableQuery()
                            .Where(x => x.RouteId == parentId)
                            .Select(x => new RouteModel
                            {
                                RouteId = x.RouteId,
                                RouteValue = x.RoutePath,
                                RouteName = x.RouteName,
                                Description = x.Description,
                                ParentRouteId = x.ParentRouteId
                            })
                            .FirstOrDefaultAsync();

                        genericRoutes.Add(parentRoute);
                    }
                }

                return genericRoutes.DistinctBy(x => x.RouteId);
            }
        }

        public async Task<PagedModel<RouteModel>> GetPagedRoutes(PagedModel<RouteModel> pagedRoutes)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleRepo = factory.GetRepository<Role>();
                _routeRepo = factory.GetRepository<Route>();
                _userRoleMappingRepo = factory.GetRepository<UserRoleMapping>();
                var query = _routeRepo.UnTrackableQuery().Where(x => x.RouteId > 0);

                if (!await this.HasSystemAdminPriviledge(_userRoleMappingRepo))
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                if (!string.IsNullOrEmpty(pagedRoutes.SearchString))
                {
                    query = query
                        .Where(x => x.RouteId > 0 &&
                            (x.RouteName.ToLower().Contains(pagedRoutes.SearchString.ToLower())
                            || x.RoutePath.ToLower().Contains(pagedRoutes.SearchString.ToLower())
                            || x.Description.ToLower().Contains(pagedRoutes.SearchString.ToLower())
                            || x.Application.ApplicationName.ToLower().Contains(pagedRoutes.SearchString.ToLower())));
                }

                pagedRoutes.SourceData = await query
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
                            RouteTypeId = x.RouteTypeId,
                            ApplicationName = x.Application.ApplicationName,
                            IsMenuRoute = x.IsMenuRoute
                        })
                        .ToListAsync();

                pagedRoutes.TotalItems = await query.CountAsync();
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
                _roleRepo = factory.GetRepository<Role>();
                _roleMappingRepo = factory.GetRepository<RoleRouteMapping>();

                int appId = await _appRepo.UnTrackableQuery()
                    .Where(x => x.ApplicationUrl == User.RequestOrigin)
                    .Select(x => x.ApplicationId)
                    .FirstOrDefaultAsync();

                Role userRole = await _roleRepo.UnTrackableQuery()
                    .FirstOrDefaultAsync(x => x.RoleId == roleId);

                if (userRole == null) throw new Exception($"No role found with identifier {roleId}");

                return await _roleMappingRepo.UnTrackableQuery()
                    .Where(roleRouteMapping => roleRouteMapping.RouteId > 0 && roleRouteMapping.RoleId == roleId)
                    .Select(x => new RouteModel
                    {
                        RouteId = x.Route.RouteId,
                        RouteName = x.Route.RouteName
                    }).ToListAsync();
            }
        }

        public async Task<List<RouteType>> GetRouteTypes()
        {
            using(var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _routeTypeRepo = factory.GetRepository<RouteType>();

                return await _routeTypeRepo.UnTrackableQuery()
                    .Where(x => x.RouteTypeId > 0)
                    .ToListAsync();
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

                Mapper.Map(routeModel, routeEntity);

                routeEntity = _routeRepo.Update(routeEntity);

                await factory.SaveChangesAsync();
                return Mapper.Map<RouteModel>(routeEntity);
            }
        }
    }
}
