using AutoMapper.Configuration.Conventions;
using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Domain;
using RedBook.Core.Models;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using System.Data;
using System.Security.Claims;

namespace Identity.Domain.Implementation
{
    public class RoleService : ServiceBase, IRoleService
    {
        private IRepositoryBase<User> _userRepo;
        private IRepositoryBase<Role> _roleRepo;
        public RoleService(
            ILogger<RoleService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        {  }

        // Testing required
        public async Task<RoleModel> AddRoleAsync(RoleModel role)
        {
            // Only an admin user has the right to create a role for only his organizations
            Role? roleEntity;
            var orgId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals("OrganizationId", StringComparison.InvariantCultureIgnoreCase))?.Value);
            var userRoleId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value);
            using(var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                var userRoleEntity = await _roleRepo.GetByIdAsync(userRoleId);
                if (userRoleEntity == null)
                    throw new ArgumentException($"Role with identifier {userRoleId} was not found");

                if (userRoleEntity.IsAdmin)
                {
                    if(userRoleId == 1 && orgId == 1)
                    {
                        roleEntity = await _roleRepo.InsertAsync(new Role
                        {
                            RoleName = role.RoleName,
                            IsAdmin = role.IsAdmin,
                            OrganizationId = role.OrganizationId
                        });
                    }
                    else
                    {
                        roleEntity = await _roleRepo.InsertAsync(new Role
                        {
                            RoleName = role.RoleName,
                            IsAdmin = role.IsAdmin,
                            OrganizationId = orgId
                        });
                    }
                }
                else
                    throw new ArgumentException("Only admin users are authorized to execute this operation");

                await transaction.SaveChangesAsync();
            }

            return Mapper.Map<RoleModel>(roleEntity);
        }
        public async Task DeleteRoleAsync(int roleId)
        {
            // If there are users under this role, this role can not be deleted
            bool roleHasUsers = _userRepo.UnTrackableQuery().Where(x => x.RoleId == roleId).Count() > 0;
            if (roleHasUsers) throw new ArgumentException("One or more users hold this role");

            // Only an admin user has the right to delete a roles
            int orgId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals("OrganizationId", StringComparison.InvariantCultureIgnoreCase))?.Value);
            int userRoleId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value);
            Role? userRoleEntity = await _roleRepo.GetByIdAsync(userRoleId);

            if (userRoleEntity == null)
                throw new ArgumentException($"Role with identifier {roleId} was not found");

            if (userRoleEntity.IsAdmin)
                throw new ArgumentException("Only admin users are authorized to execute this operation");

            // An user can delete the roles of his organization only
            Role? roleToDelete = await _roleRepo.GetByIdAsync(roleId);

            using (var transaction = UnitOfWorkManager.Begin())
            {
                await _roleRepo.DeleteAsync(roleToDelete);
                await transaction.SaveChangesAsync();
            }
        }
        public async Task<RoleModel> GetRoleAsync(int roleId)
        {
            // Only an admin user has the right to delete a roles
            Role? userRoleEntity;
            int userRoleId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role, StringComparison.InvariantCultureIgnoreCase))?.Value);
            int orgId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals("OrganizationId", StringComparison.InvariantCultureIgnoreCase))?.Value);
            using(var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                userRoleEntity = await _roleRepo.GetByIdAsync(userRoleId);
            }

            if (userRoleEntity == null) throw new ArgumentException($"Role with identifier {userRoleId} was not found");

            if (userRoleEntity.IsAdmin)
                throw new ArgumentException("Only admin users are authorized to execute this operation");

            return Mapper.Map<RoleModel>(userRoleEntity);
        }
        public async Task<PagedModel<RoleModel>> GetRolesAsync(PagedModel<RoleModel> pagedRoleModel)
        {
            int orgId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals("OrganizationId", StringComparison.InvariantCultureIgnoreCase))?.Value);
            int roleId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value);

            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _roleRepo = unitOfWork.GetRepository<Role>();
                Role? requestingUserRole = await _roleRepo.GetByIdAsync(roleId);
                if (requestingUserRole == null) throw new ArgumentException($"Role with identifier {roleId} not found");
                List<RoleModel> roleModelCollection = new List<RoleModel>();
                if (requestingUserRole.IsAdmin)
                {
                    pagedRoleModel.SourceData = _roleRepo.TrackableQuery()
                        .Where(x => x.OrganizationId == orgId)
                        .Select(x => new RoleModel
                        {
                            Id = x.RoleId,
                            RoleName = x.RoleName,
                            OrganizationId = orgId
                        })
                        .ToList();
                }
                else
                    throw new ArgumentException($"Only Admin Users are allowed to perform this operation");

            }

            return pagedRoleModel;
        }
        public async Task<RoleModel> UpdateRoleAsync(RoleModel role)
        {
            // Only an admin user has the right to delete a roles
            var orgId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals("OrganizationId", StringComparison.InvariantCultureIgnoreCase))?.Value);
            var userRoleId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role, StringComparison.InvariantCultureIgnoreCase))?.Value);

            Role? roleEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                roleEntity = await _roleRepo.GetByIdAsync(role.Id);
                if (roleEntity == null) throw new ArgumentException($"Role with identifier {role.Id} was not found");
                roleEntity.RoleName = role.RoleName;
                roleEntity.IsAdmin = role.IsAdmin;
                roleEntity.OrganizationId = orgId;
                roleEntity = _roleRepo.Update(roleEntity);
                await transaction.SaveChangesAsync();
            }

            return Mapper.Map<RoleModel>(roleEntity);
        }
    }
}
