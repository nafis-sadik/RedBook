﻿using Identity.Data.CommonConstant;
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

                var routeQuery = _routeRepo.UnTrackableQuery().Where(x => x.ApplicationId == appId && x.IsMenuRoute);

                // If the user is a sys admin user, return all available routes
                bool hasDefaultRouteFlag = false;
                if (requesterRoles.Any(x => x.IsSystemAdmin))
                {
                    routeQuery = routeQuery.Where(x => x.RouteId > 0);
                    hasDefaultRouteFlag = true;
                }
                else
                {
                    if (requesterRoles.Any(x => x.IsOwner))
                    {
                        routeQuery = routeQuery.Where(x =>
                            x.RouteTypeId == RouteTypeConsts.OrganizationOwner.RouteTypeId
                            || x.RouteTypeId == RouteTypeConsts.AdminRoute.RouteTypeId
                            || x.RouteTypeId == RouteTypeConsts.GenericRoute.RouteTypeId);

                        hasDefaultRouteFlag = true;
                    }

                    if (requesterRoles.Any(x => x.IsAdmin))
                    {
                        routeQuery = routeQuery.Where(x =>
                            x.RouteTypeId == RouteTypeConsts.AdminRoute.RouteTypeId
                            || x.RouteTypeId == RouteTypeConsts.GenericRoute.RouteTypeId);

                        hasDefaultRouteFlag = true;
                    }

                    if (requesterRoles.Any(x => x.IsRetailer))
                    {
                        routeQuery = routeQuery.Where(x => x.RouteTypeId == RouteTypeConsts.RetailerRoute.RouteTypeId);
                        hasDefaultRouteFlag = true;
                    }

                }

                List<RouteModel> genericRoutes = await routeQuery.Select(x => new RouteModel
                {
                    RouteId = x.RouteId,
                    RouteValue = x.RoutePath,
                    RouteName = x.RouteName,
                    Description = x.Description,
                    ParentRouteId = x.ParentRouteId
                }).ToListAsync();

                if (!hasDefaultRouteFlag)
                    genericRoutes = new List<RouteModel>();

                // Custom role
                var roleWiseRouteQuery = _roleRouteMappingRepo.UnTrackableQuery();
                foreach (Role requesterRole in requesterRoles)
                {
                    roleWiseRouteQuery = roleWiseRouteQuery.Where(x => x.Route.ApplicationId == requesterRole.ApplicationId);
                }

                List<RouteModel> customRoleRoutes = await roleWiseRouteQuery.Distinct()
                    .Select(x => new RouteModel
                    {
                        RouteId = x.RouteId,
                        RouteValue = x.Route.RoutePath,
                        RouteName = x.Route.RouteName,
                        Description = x.Route.Description,
                        ParentRouteId = x.Route.ParentRouteId
                    })
                    .ToListAsync();

                genericRoutes.AddRange(customRoleRoutes);
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
                _roleRepo = factory.GetRepository<Role>();
                _roleMappingRepo = factory.GetRepository<RoleRouteMapping>();

                int appId = await _appRepo.UnTrackableQuery()
                    .Where(x => x.ApplicationUrl == User.RequestOrigin)
                    .Select(x => x.ApplicationId)
                    .FirstOrDefaultAsync();

                Role userRole = await _roleRepo.UnTrackableQuery()
                    .FirstOrDefaultAsync(x => x.RoleId == roleId);

                if (userRole == null) throw new Exception($"No role found with identifier {roleId}");

                if (userRole.IsSystemAdmin)
                {
                    return await _routeRepo.UnTrackableQuery()
                        .Where(x => x.IsMenuRoute && x.ApplicationId == appId)
                        .Select(x => new RouteModel
                        {
                            RouteId = x.RouteId,
                            RouteName = x.RouteName
                        }).ToListAsync();
                }
                else if (userRole.IsAdmin)
                {
                    return await _routeRepo.UnTrackableQuery()
                        .Where(x => x.IsMenuRoute && x.ApplicationId == appId && x.RouteTypeId == RouteTypeConsts.AdminRoute.RouteTypeId || x.RouteTypeId == RouteTypeConsts.GenericRoute.RouteTypeId)
                        .Select(x => new RouteModel
                        {
                            RouteId = x.RouteId,
                            RouteName = x.RouteName
                        }).ToListAsync();
                }
                else
                {
                    var query = _roleMappingRepo.UnTrackableQuery().Where(x => x.RouteId > 0);

                    if (userRole.IsRetailer)
                        query = query.Where(x => x.Route.RouteTypeId == RouteTypeConsts.RetailerRoute.RouteTypeId);
                    else
                        query = query.Where(x => x.RoleId == roleId);

                    return await query
                        .Select(x => new RouteModel
                        {
                            RouteId = x.Route.RouteId,
                            RouteName = x.Route.RouteName
                        }).ToListAsync();
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
