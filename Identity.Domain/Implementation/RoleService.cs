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
        private readonly IRepositoryBase<User> _userRepo;
        private readonly IRepositoryBase<Role> _roleRepo;
        private readonly IRepositoryBase<OrganizationRoleMapping> _roleOrganizationMappingRepo;
        public RoleService(
            ILogger<RoleService> logger,
            IObjectMapper mapper,
            IUnitOfWork unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        {
            _userRepo = unitOfWork.GetRepository<User>();
            _roleRepo = unitOfWork.GetRepository<Role>();
            _roleOrganizationMappingRepo = unitOfWork.GetRepository<OrganizationRoleMapping>();
        }

        // Testing required
        public async Task<RoleModel> AddRoleAsync(RoleModel role)
        {
            // Only an admin user has the right to create a role for only his organizations
            var orgId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals("OrganizationId", StringComparison.InvariantCultureIgnoreCase))?.Value);
            var userRoleId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value);
            var userRoleEntity = await _roleRepo.GetByIdAsync(userRoleId);

            if (userRoleEntity == null)
                throw new ArgumentException($"Invalid user role");


            Role? roleEntity;

            using(var transaction = UnitOfWorkManager.Begin())
            {
                int[] rolesOfUserOrg = _roleOrganizationMappingRepo.UnTrackableQuery().Where(x => x.OrganizationId == orgId).Select(x => x.RoleId).ToArray();
                roleEntity = _roleRepo.UnTrackableQuery().FirstOrDefault(x => x.RoleName == role.RoleName && rolesOfUserOrg.Contains(x.Id));

                if (roleEntity != null)
                    throw new ArgumentException($"Role already exists for your organization");

                if (userRoleEntity.RoleName.ToLower().Equals(CommonConstants.GenericRoles.Admin.ToLower()))
                {
                    roleEntity = await _roleRepo.InsertAsync(new Role
                    {
                        IsGenericRole = false,
                        RoleName = role.RoleName
                    });
                }
                else if (userRoleEntity.RoleName.ToLower().Equals(CommonConstants.GenericRoles.SystemAdmin.ToLower()))
                {
                    roleEntity = await _roleRepo.InsertAsync(new Role
                    {
                        IsGenericRole = true,
                        RoleName = role.RoleName
                    });
                }
                else
                    throw new ArgumentException("Only admin users are authorized to execute this operation");

                await transaction.SaveChangesAsync();

                await _roleOrganizationMappingRepo.InsertAsync(new OrganizationRoleMapping
                {
                    OrganizationId = orgId,
                    RoleId = roleEntity.Id
                });

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

            if (!userRoleEntity.RoleName.ToLower().Equals(CommonConstants.GenericRoles.Admin.ToLower()) && !userRoleEntity.RoleName.ToLower().Equals(CommonConstants.GenericRoles.SystemAdmin.ToLower()))
                throw new ArgumentException("Only admin users are authorized to execute this operation");

            // An user can delete the roles of his organization only
            Role? roleToDelete = await _roleRepo.GetByIdAsync(roleId);
            if (roleToDelete.OrganizationRoleMappings.Where(x => x.OrganizationId.Equals(orgId)).Count() <= 0)
                throw new ArgumentException("Users can delete only the roles of their own organization");

            using (var transaction = UnitOfWorkManager.Begin())
            {
                await _roleRepo.DeleteAsync(roleToDelete);
                await transaction.SaveChangesAsync();
            }
        }
        public async Task<RoleModel> GetRoleAsync(int roleId)
        {
            // Only an admin user has the right to delete a roles
            int userRoleId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role, StringComparison.InvariantCultureIgnoreCase))?.Value);
            int orgId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals("OrganizationId", StringComparison.InvariantCultureIgnoreCase))?.Value);
            Role? roleEntity = await _roleRepo.GetByIdAsync(roleId);
            Role? userRoleEntity = await _roleRepo.GetByIdAsync(userRoleId);

            if (roleEntity.OrganizationRoleMappings.Where(x => x.Organization.Id.Equals(orgId)).Count() <= 0 && roleEntity.IsGenericRole == false)
                throw new ArgumentException($"Role identified by {orgId} does not belong to your organization");

            if (!userRoleEntity.RoleName.ToLower().Equals(CommonConstants.GenericRoles.Admin.ToLower()) && !userRoleEntity.RoleName.ToLower().Equals(CommonConstants.GenericRoles.SystemAdmin.ToLower()))
                throw new ArgumentException("Only admin users are authorized to execute this operation");

            return Mapper.Map<RoleModel>(roleEntity);
        }
        public PagedModel<RoleModel> GetRolesAsync(PagedModel<RoleModel> pagedRoleModel)
        {
            int orgId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals("OrganizationId", StringComparison.InvariantCultureIgnoreCase))?.Value);
            //var roleOrgMapping = _roleOrganizationMappingRepo.UnTrackableQuery().Where(x => x.OrganizationId == orgId).Select(x => x.RoleId);
            var roleEntities = _roleRepo.TrackableQuery().Where(x => _roleOrganizationMappingRepo.UnTrackableQuery().Where(x => x.OrganizationId == orgId).Select(x => x.RoleId).Contains(x.Id)).ToList();
            
            List<RoleModel> roleModelCollection = new List<RoleModel>();
            
            foreach (var role in roleEntities)
            {
                roleModelCollection.Add(Mapper.Map<RoleModel>(role));
            }

            pagedRoleModel.Items = roleModelCollection;

            return pagedRoleModel;
        }
        public async Task<RoleModel> UpdateRoleAsync(RoleModel role)
        {
            // Only an admin user has the right to delete a roles
            var orgId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals("OrganizationId", StringComparison.InvariantCultureIgnoreCase))?.Value);
            var userRoleId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role, StringComparison.InvariantCultureIgnoreCase))?.Value);

            Role roleEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                roleEntity = await _roleRepo.GetByIdAsync(role.Id);
                roleEntity.RoleName = role.RoleName;
                roleEntity = _roleRepo.Update(roleEntity);
                await transaction.SaveChangesAsync();
            }

            return Mapper.Map<RoleModel>(roleEntity);
        }
    }
}
