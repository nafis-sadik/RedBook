using Identity.Data.Entities;
using Identity.Data.Models;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using RedBook.Core.Repositories;
using Identity.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Domain;
using System.Data;

namespace Identity.Domain.Implementation
{
    public class RoleService : ServiceBase, IRoleService
    {
        private IRepositoryBase<Role> _roleRepo;
        private IRepositoryBase<UserRoleMapping> _userRoleRepo;
        private IRepositoryBase<RoleRouteMapping> _roleRouteMappingRepo;

        public RoleService(
            ILogger<RoleService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        { }
        
        // Org admin only
        public async Task<RoleModel> AddRoleAsync(RoleModel role)
        {
            if (role.OrganizationId <= 0) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);

            using(var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleRepo = factory.GetRepository<Role>();
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                if (!await this.HasAdminPriviledge(_userRoleRepo, role.OrganizationId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                Role roleEntity = await _roleRepo.InsertAsync(new Role
                {
                    RoleName = role.RoleName,
                    IsAdmin = role.IsAdmin,
                    OrganizationId = role.OrganizationId
                });

                await factory.SaveChangesAsync();

                return Mapper.Map<RoleModel>(roleEntity);
            }
        }

        // Org admin only
        public async Task DeleteRoleAsync(int roleId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleRepo = factory.GetRepository<Role>();
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                Role role = await _roleRepo.GetAsync(roleId);
                if (role == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);
                
                if (!await this.HasAdminPriviledge(_userRoleRepo, role.OrganizationId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);
                
                _roleRepo.Delete(role);
                await factory.SaveChangesAsync();
            }
        }

        // Org admin only
        public async Task<IEnumerable<RoleModel>> GetOrganizationRoles(int orgId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleRepo = factory.GetRepository<Role>();
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                if (!await this.HasAdminPriviledge(_userRoleRepo, orgId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                return await _roleRepo.UnTrackableQuery()
                            .Where(x => x.OrganizationId == orgId)
                            .Select(x => new RoleModel
                            {
                                RoleId = x.RoleId,
                                OrganizationId = x.OrganizationId,
                                IsAdmin = x.IsAdmin == true,
                                RoleName = x.RoleName,
                            })
                            .ToListAsync();
            }
        }

        public async Task InvertRouteRoleMapping(int roleId, int routeId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleRouteMappingRepo = factory.GetRepository<RoleRouteMapping>();

                RoleRouteMapping? existingPermission = await _roleRouteMappingRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.RouteId == routeId && x.RoleId == roleId);
                
                if (existingPermission != null) {
                    _roleRouteMappingRepo.Delete(existingPermission);
                } else {
                    await _roleRouteMappingRepo.InsertAsync(new RoleRouteMapping
                    {
                        RoleId = roleId,
                        RouteId = routeId
                    });
                }

                await factory.SaveChangesAsync();
            }
        }

        // Org admin only
        public async Task<RoleModel> UpdateRoleAsync(RoleModel role)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleRepo = factory.GetRepository<Role>();
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                if (!await this.HasAdminPriviledge(_userRoleRepo, role.OrganizationId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                Role? roleEntity = await _roleRepo.GetAsync(role.RoleId);
                if (roleEntity == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);
                Mapper.Map(role, roleEntity);
                roleEntity = _roleRepo.Update(roleEntity);
                await factory.SaveChangesAsync();

                return Mapper.Map<RoleModel>(roleEntity);
            }
        }

        public async Task<int[]> GetOrganizationsAllowedToUserByRoute(int userId, int routeId)
        {
            using(var unitOfWord = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _userRepo = unitOfWord.GetRepository<User>();
                _userRoleRepo = unitOfWord.GetRepository<UserRoleMapping>();
                _roleRouteMappingRepo = unitOfWord.GetRepository<RoleRouteMapping>();

                User usr = await _userRepo.GetAsync(userId);
                if (usr == null) throw new ArgumentException("Resource not found");

                // Need to check if is admin
                int[] roleOrfids = await _roleRouteMappingRepo
                    .UnTrackableQuery()
                    .Where(x => x.RouteId == routeId)
                    .Select(x => x.Role.OrganizationId)
                    .ToArrayAsync();

                int[] userOrgIds = await _userRoleRepo
                    .UnTrackableQuery()
                    .Where(x => x.UserId == userId && x.RoleId == routeId)
                    .Select(x => x.Role.OrganizationId)
                    .ToArrayAsync();

                List<int> responseIds = new List<int>();
                foreach(var userOrgId in userOrgIds)
                {
                    if(roleOrfids.Contains(userOrgId))
                        responseIds.Add(userOrgId);
                }

                return responseIds.ToArray();
            }
        }
    }
}
