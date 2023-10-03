using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Domain;
using RedBook.Core.Models;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using System.Security.Claims;

namespace Identity.Domain.Implementation
{
    public class OrganizationService : ServiceBase, IOrganizationService
    {
        private IRepositoryBase<Role> _roleRepo;
        private IRepositoryBase<UserRole> _userRoleRepo;
        private IRepositoryBase<Organization> _orgRepo;
        private IRepositoryBase<RoleRouteMapping> _roleRouteMappingRepo;
        private int[] requesterRoleIds;

        public OrganizationService(
            ILogger<ApplicationService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        {
            string? roleIds = User?.Claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role, StringComparison.InvariantCultureIgnoreCase))?.Value;
            if (!string.IsNullOrEmpty(roleIds))
            {
                string[] strArray = roleIds.Split(',');
                requesterRoleIds = Array.ConvertAll(strArray, int.Parse);
            }
            else
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);
        }

        public async Task<OrganizationModel> AddOrganizationAsync(OrganizationModel organizationModel)
        {
            Organization? orgEntity = null;

            string? requesterRoleIdStr = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;
            if (string.IsNullOrEmpty(requesterRoleIdStr))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            //int[] requesterRoleIds = requesterRoleIdStr.Split(',')

            string? requesterUserId = User?.Claims.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value;
            if (string.IsNullOrEmpty(requesterUserId))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _orgRepo = transaction.GetRepository<Organization>();
                _userRoleRepo = transaction.GetRepository<UserRole>();

                foreach (int requesterRoleId in requesterRoleIds)
                {
                    // Get role to check permissions
                    Role? requesterRole = await _roleRepo.Get(requesterRoleId);
                    if (requesterRole == null)
                        throw new ArgumentException($"Requester role with identifier {requesterRoleId} was not found");

                    // A retailer can add new organization to onboard new customer
                    // An admin user can add new organization to manage multiple businesses from redbook
                    if (requesterRole.IsAdmin || requesterRole.IsRetailer)
                    {
                        orgEntity = Mapper.Map<Organization>(organizationModel);
                        orgEntity.OrganizationId = 0;
                        orgEntity.CreateDate = DateTime.UtcNow;
                        orgEntity.CreatedBy = requesterUserId;
                        orgEntity = await _orgRepo.InsertAsync(orgEntity);
                    }
                }

                await transaction.SaveChangesAsync();

                Role? adminRoleForNewOrg = null;
                if (orgEntity != null)
                {
                    adminRoleForNewOrg = await _roleRepo.InsertAsync(new Role
                    {
                        IsAdmin = true,
                        IsRetailer = false,
                        RoleName = "Admin",
                        IsSystemAdmin = false,
                        OrganizationId = orgEntity.OrganizationId,
                    });
                }

                await transaction.SaveChangesAsync();

                UserRole? userRoleMapping = null;
                string? creatingUserId = orgEntity?.CreatedBy;
                if (adminRoleForNewOrg != null && !string.IsNullOrEmpty(creatingUserId))
                {
                    userRoleMapping = await _userRoleRepo.InsertAsync(new UserRole
                    {
                        RoleId = adminRoleForNewOrg.RoleId,
                        UserId = creatingUserId,
                    });
                }

                await transaction.SaveChangesAsync();
            }

            if (orgEntity != null)
                organizationModel = Mapper.Map<OrganizationModel>(orgEntity);
            else
                throw new ArgumentException("Failed to add new organization, internal error occured");

            return organizationModel;
        }

        public async Task DeleteOrganizationAsync(int OrganizationId)
        {
            string? requesterUserStr = User?.Claims.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value;

            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _orgRepo = transaction.GetRepository<Organization>();
                _userRoleRepo = transaction.GetRepository<UserRole>();
                _roleRouteMappingRepo = transaction.GetRepository<RoleRouteMapping>();

                // Admin priviledge check
                int userRoleCounts = await _userRoleRepo
                    .UnTrackableQuery()
                    .Where(x => x.UserId == requesterUserStr && x.Role.IsAdmin && x.Role.OrganizationId == OrganizationId)
                    .CountAsync();

                if (userRoleCounts <= 0)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                int[]? orgRolesIds = await _roleRepo.UnTrackableQuery().Where(x => x.OrganizationId == OrganizationId).Select(x => x.RoleId).ToArrayAsync();
                foreach(int orgRoleId in orgRolesIds)
                {
                    // Delete User Role Mapping Records
                    int[]? userRoleMappingIds = await _userRoleRepo.UnTrackableQuery().Where(x => x.RoleId == orgRoleId).Select(x => x.UserRoleId).ToArrayAsync();
                    foreach(int mappingId in userRoleMappingIds)
                    {
                        await _userRoleRepo.DeleteAsync(mappingId);
                    }
                    await transaction.SaveChangesAsync();

                    // Delete Role Route Mapping Records
                    int[]? roleRouteMappingIds = await _roleRouteMappingRepo.UnTrackableQuery().Where(x => x.RoleId == orgRoleId).Select(x => x.MappingId).ToArrayAsync();
                    foreach (int mappingId in roleRouteMappingIds)
                    {
                        await _roleRouteMappingRepo.DeleteAsync(mappingId);
                    }
                    await transaction.SaveChangesAsync();

                    await _roleRepo.DeleteAsync(orgRoleId);
                    await transaction.SaveChangesAsync();
                }

                await _orgRepo.DeleteAsync(OrganizationId);
                await transaction.SaveChangesAsync();
            }
        }

        public async Task<OrganizationModel> GetOrganizationAsync(int OrganizationId)
        {
            Organization? orgEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _orgRepo = transaction.GetRepository<Organization>();
                orgEntity = await _orgRepo.Get(OrganizationId);
            }
            if (orgEntity == null) throw new ArgumentException($"Organization with identifier {OrganizationId} was not found");
            return Mapper.Map<OrganizationModel>(orgEntity);
        }

        public async Task<IEnumerable<OrganizationModel>> GetOrganizationsAsync()
        {
            string? requesterUserStr = User?.Claims.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value;

            if (string.IsNullOrEmpty(requesterUserStr))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            List<OrganizationModel> organizationModels = new List<OrganizationModel>();
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _orgRepo = transaction.GetRepository<Organization>();
                _userRoleRepo = transaction.GetRepository<UserRole>();

                int[]? roleIds = await _userRoleRepo.UnTrackableQuery().Where(x => x.UserId == requesterUserStr).Select(x => x.RoleId).ToArrayAsync();

                foreach (int roleId in roleIds)
                {
                    Role? roleEntity = await _roleRepo.Get(roleId);

                    if (roleEntity == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

                    if(roleEntity.IsAdmin)
                    {
                        organizationModels.AddRange(await _orgRepo.UnTrackableQuery()
                            .Where(x => x.OrganizationId == roleEntity.OrganizationId)
                            .Select(x => new OrganizationModel
                            {
                                OrganizationId = x.OrganizationId,
                                OrganizationName = x.OrganizationName
                            }).ToArrayAsync());
                    }
                }
            }

            return organizationModels;
        }

        public async Task<PagedModel<OrganizationModel>> GetPagedOrganizationsAsync(PagedModel<OrganizationModel> pagedOrganizationModel)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _orgRepo = transaction.GetRepository<Organization>();
                pagedOrganizationModel.SourceData = await _orgRepo.UnTrackableQuery()
                    .Where(x => x.OrganizationId > 0)
                    .Skip(pagedOrganizationModel.Skip)
                    .Take(pagedOrganizationModel.PageSize)
                    .Select(x => new OrganizationModel {
                        OrganizationId = x.OrganizationId,
                        OrganizationName = x.OrganizationName
                    }).ToListAsync();
            }

            return pagedOrganizationModel;
        }

        public async Task<OrganizationModel> UpdateOrganizationAsync(OrganizationModel organizationModel)
        {
            Organization? orgEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _orgRepo = transaction.GetRepository<Organization>();
                orgEntity = await _orgRepo.Get(organizationModel.OrganizationId);
                if (orgEntity == null) throw new ArgumentException($"Organization with identifier {organizationModel.OrganizationId} was not found");
                orgEntity = Mapper.Map(organizationModel, orgEntity);
                orgEntity = _orgRepo.Update(orgEntity);
                await transaction.SaveChangesAsync();
            }
            return Mapper.Map<OrganizationModel>(orgEntity);
        }
    }
}
