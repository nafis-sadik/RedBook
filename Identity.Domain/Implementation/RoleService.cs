using Identity.Data.Entities;
using Identity.Data.Migrations;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Domain;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using System.Data;

namespace Identity.Domain.Implementation
{
    public class RoleService : ServiceBase, IRoleService
    {
        private IRepositoryBase<Role> _roleRepo;
        private IRepositoryBase<UserRole> _userRoleMappingRepo;

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
            using(var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _userRoleMappingRepo = transaction.GetRepository<UserRole>();

                if (!await this.HasAdminPriviledge(_roleRepo, role.OrganizationId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                Role roleEntity = await _roleRepo.InsertAsync(new Role
                {
                    RoleName = role.RoleName,
                    IsAdmin = role.IsAdmin,
                    OrganizationId = role.OrganizationId
                });

                await transaction.SaveChangesAsync();

                return Mapper.Map<RoleModel>(roleEntity);
            }
        }

        // Org admin only
        public async Task DeleteRoleAsync(int roleId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _userRoleMappingRepo = transaction.GetRepository<UserRole>();

                Role? requestingUserRoleEntity = await _roleRepo.GetAsync(roleId);
                if (requestingUserRoleEntity == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);
                
                if (!await this.HasAdminPriviledge(_roleRepo, requestingUserRoleEntity.OrganizationId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);
                
                _roleRepo.Delete(requestingUserRoleEntity);
                await transaction.SaveChangesAsync();
            }
        }

        // Org admin only
        public async Task<IEnumerable<RoleModel>> GetOrganizationRoles(int orgId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();

                if (!await this.HasAdminPriviledge(_roleRepo, orgId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                return await _roleRepo.UnTrackableQuery()
                            .Where(x => x.OrganizationId == orgId)
                            .Select(x => new RoleModel
                            {
                                RoleId = x.RoleId,
                                OrganizationId = x.OrganizationId,
                                RoleName = x.RoleName,
                            })
                            .ToListAsync();
            }
        }

        // Org admin only
        public async Task<RoleModel> UpdateRoleAsync(RoleModel role)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();

                if (!await this.HasAdminPriviledge(_roleRepo, role.OrganizationId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                Role? roleEntity = await _roleRepo.GetAsync(role.RoleId);
                if (roleEntity == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);
                roleEntity.RoleName = role.RoleName;
                roleEntity.OrganizationId = role.OrganizationId;
                roleEntity = _roleRepo.Update(roleEntity);
                await transaction.SaveChangesAsync();

                return Mapper.Map<RoleModel>(roleEntity);
            }
        }
    }
}
