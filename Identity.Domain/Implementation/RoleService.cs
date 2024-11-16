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
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using System.Data;

namespace Identity.Domain.Implementation
{
    public class RoleService : ServiceBase, IRoleService
    {
        public RoleService(
            ILogger<RoleService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor)
        { }

        // Org admin only
        // review required
        public async Task<RoleModel> AddRoleAsync(RoleModel role)
        {
            if (!HttpContextAccessor.Request.Headers.TryGetValue("Origin", out var originStrPrimitives))
                throw new ArgumentException("Invalid Origin");

            if (role.OrganizationId <= 0) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);

            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _roleRepo = factory.GetRepository<Role>();
                var _routeRepo = factory.GetRepository<Route>();
                var _appRepo = factory.GetRepository<Application>();
                var _userRoleRepo = factory.GetRepository<UserRoleMapping>();
                var _roleRouteMappingRepo = factory.GetRepository<RoleRouteMapping>();

                string originUrl = originStrPrimitives.ToString();

                int appId = await _appRepo.UnTrackableQuery()
                    .Where(x => x.ApplicationUrl.ToLower().Equals(originUrl.ToLower()))
                    .Select(x => x.ApplicationId)
                    .FirstOrDefaultAsync();

                if (appId <= 0) throw new ArgumentException("Unknown Origin");

                if (!await this.IsAdminOf(_userRoleRepo, role.OrganizationId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                Role roleEntity = await _roleRepo.InsertAsync(new Role
                {
                    RoleName = role.RoleName,
                    OrganizationId = role.OrganizationId,
                    ApplicationId = appId,
                    IsAdmin = role.IsAdmin,
                    //IsRetailer = role.IsRetailer
                });
                await factory.SaveChangesAsync();

                if (role.IsAdmin)
                {
                    List<int> routeIdList = await _routeRepo.UnTrackableQuery()
                        .Where(r => r.RouteTypeId == RouteTypeConsts.AdminRoute.RouteTypeId)
                        .Select(r => r.RouteId)
                        .ToListAsync();

                    foreach (int routeId in routeIdList)
                    {
                        await _roleRouteMappingRepo.InsertAsync(new RoleRouteMapping
                        {
                            RoleId = roleEntity.RoleId,
                            RouteId = routeId
                        });
                    }

                    await factory.SaveChangesAsync();
                }

                return Mapper.Map<RoleModel>(roleEntity);
            }
        }

        // Org admin only
        public async Task DeleteRoleAsync(int roleId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _roleRepo = factory.GetRepository<Role>();
                var _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                Role role = await _roleRepo.UnTrackableQuery()
                    .Where(x => x.RoleId == roleId)
                    .Select(r => new Role
                    {
                        RoleId = r.RoleId,
                        OrganizationId = r.OrganizationId
                    })
                    .FirstOrDefaultAsync();

                if (role == null || role.OrganizationId == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);
                else
                {
                    if (!await this.IsAdminOf(_userRoleRepo, (int)role.OrganizationId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                    _roleRepo.Delete(role);
                    await factory.SaveChangesAsync();
                }
            }
        }

        // Org admin only
        public async Task<IEnumerable<RoleModel>> GetOrganizationRoles(int orgId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _roleRepo = factory.GetRepository<Role>();
                var _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                if (!await this.IsAdminOf(_userRoleRepo, orgId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                return await _roleRepo.UnTrackableQuery()
                            .Where(x => x.OrganizationId == orgId)
                            .Select(x => new RoleModel
                            {
                                RoleId = x.RoleId,
                                OrganizationId = (int)x.OrganizationId,
                                IsAdmin = x.IsAdmin,
                                RoleName = x.RoleName,
                            })
                            .ToListAsync();
            }
        }

        // review
        public async Task<bool> InvertRouteRoleMapping(int roleId, int routeId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _routeRepo = factory.GetRepository<Route>();
                var _roleRouteMappingRepo = factory.GetRepository<RoleRouteMapping>();

                RoleRouteMapping? existingPermission = await _roleRouteMappingRepo.UnTrackableQuery()
                    .Where(x => x.RouteId == routeId && x.RoleId == roleId)
                    .Select(roleRouteMapping => new RoleRouteMapping
                    {
                        MappingId = roleRouteMapping.MappingId,
                        Route = new Route {
                            RouteTypeId = roleRouteMapping.Route.RouteTypeId
                        },
                        Role = new Role { 
                            IsAdmin = roleRouteMapping.Role.IsAdmin,
                            IsSystemAdmin = roleRouteMapping.Role.IsSystemAdmin,
                            IsRetailer = roleRouteMapping.Role.IsRetailer,
                        }
                    }).FirstOrDefaultAsync();

                bool response = false;
                if (existingPermission != null) {
                    if (existingPermission.Route.RouteTypeId == RouteTypeConsts.AdminRoute.RouteTypeId) {
                        throw new ArgumentException("Admin routes can not be unmapped from admin roles");
                    }

                    if (existingPermission.Route.RouteTypeId == RouteTypeConsts.RetailerRoute.RouteTypeId){
                        throw new ArgumentException("Retailer routes can not be unmapped from retailer roles");
                    }

                    await _roleRouteMappingRepo.DeleteAsync(existingPermission.MappingId);
                } else {
                    await _roleRouteMappingRepo.InsertAsync(new RoleRouteMapping
                    {
                        RoleId = roleId,
                        RouteId = routeId
                    });

                    response = true;
                }

                await factory.SaveChangesAsync();
                return response;
            }
        }

        // Org admin only
        public async Task<RoleModel> UpdateRoleAsync(RoleModel role)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _roleRepo = factory.GetRepository<Role>();
                var _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                if (!await this.IsAdminOf(_userRoleRepo, role.OrganizationId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                Role? roleEntity = await _roleRepo.GetAsync(role.RoleId);
                if (roleEntity == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);
                Mapper.Map(role, roleEntity);
                roleEntity = _roleRepo.Update(roleEntity);
                await factory.SaveChangesAsync();

                return Mapper.Map<RoleModel>(roleEntity);
            }
        }


        /// <summary>
        /// Gets the organizations that the user is allowed to access for the specified route.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="routeId">The ID of the route.</param>
        /// <returns>An array of organization IDs that the user is allowed to access for the specified route.</returns>
        public async Task<int[]> GetOrganizationsAllowedToUserByRoute(int userId, int routeId)
        {
            throw new NotImplementedException();
            using (var unitOfWord = UnitOfWorkManager.GetRepositoryFactory())
            {
                // var _userRepo = unitOfWord.GetRepository<User>();
                // var _userRoleRepo = unitOfWord.GetRepository<UserRoleMapping>();
                // var _roleRouteMappingRepo = unitOfWord.GetRepository<RoleRouteMapping>();
                // var _userRoleMappings = unitOfWord.GetRepository<UserRoleMapping>();

                // User usr = await _userRepo.GetAsync(userId);
                // if (usr == null) throw new ArgumentException("Resource not found");

                // var data = _userRoleMappings
                //     .UnTrackableQuery()
                //     .Where(x => x.UserId == userId);

                // // Get list of organization ids that have access to the route
                // int?[] orgIds = await _roleRouteMappingRepo
                //     .UnTrackableQuery()
                //     .Where(x => x.RouteId == routeId)
                //     .Select(x => x.Role.OrganizationId)
                //     .ToArrayAsync();

                // int[] userOrgIds = await _userRoleRepo
                //     .UnTrackableQuery()
                //     .Where(x => x.UserId == userId && x.RoleId == routeId)
                //     .Select(x => x.Role.OrganizationId)
                //     .ToArrayAsync();

                // List<int> responseIds = new List<int>();
                // foreach(var userOrgId in userOrgIds)
                // {
                //     if(roleOrfids.Contains(userOrgId))
                //         responseIds.Add(userOrgId);
                // }

                // return responseIds.ToArray();
            }
        }
    }
}
