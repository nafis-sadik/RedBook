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

namespace Identity.Domain.Implementation
{
    public class OrganizationService : ServiceBase, IOrganizationService
    {
        private IRepositoryBase<Role> _roleRepo;
        private IRepositoryBase<User> _userRepo;
        private IRepositoryBase<Organization> _orgRepo;
        private IRepositoryBase<UserRole> _userRoleRepo;
        private IRepositoryBase<RoleRouteMapping> _roleRouteMappingRepo;

        public OrganizationService(
            ILogger<ApplicationService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        { }

        public async Task<OrganizationModel> AddOrganizationAsync(OrganizationModel organizationModel)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _orgRepo = transaction.GetRepository<Organization>();
                _userRoleRepo = transaction.GetRepository<UserRole>();

                // Creating the new organization
                Organization orgEntity = Mapper.Map<Organization>(organizationModel);

                orgEntity.OrganizationId = 0;
                orgEntity.CreateDate = DateTime.UtcNow;
                orgEntity.CreatedBy = User.UserId;
                orgEntity = await _orgRepo.InsertAsync(orgEntity);
                await transaction.SaveChangesAsync();

                // Creating admin role for the organization
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

                    await transaction.SaveChangesAsync();
                }
                else
                    throw new ArgumentException("Failed to add new organization, internal error occured");

                // If the user is not retail user, the org is being created from settings page by an admin user as only admin users have access to that page
                if (await this.HasRetailerPriviledge(_roleRepo)) {
                    await _userRoleRepo.InsertAsync(new UserRole
                    {
                        RoleId = adminRoleForNewOrg.RoleId,
                        UserId = User.UserId,
                    });

                    await transaction.SaveChangesAsync();
                }

                return Mapper.Map<OrganizationModel>(orgEntity);
            }
        }

        public async Task AddUserToBusiness(UserModel userModel)
        {
            using(var transaction = UnitOfWorkManager.Begin())
            {
                _userRepo = transaction.GetRepository<User>();
                _userRoleRepo = transaction.GetRepository<UserRole>();

                User? userEntity = await _userRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.Email == userModel.Email);
                if(userEntity == null)
                {
                    userEntity = await _userRepo.InsertAsync(new User {
                        UserId = Guid.NewGuid().ToString(),
                        FirstName = userModel.FirstName,
                        LastName = userModel.LastName,
                        Email = userModel.Email,
                        UserName = userModel.UserName,
                        Status = CommonConstants.StatusTypes.Active.ToString(),
                        Password = CommonConstants.PasswordConfig.DefaultPassword
                    });

                    await transaction.SaveChangesAsync();
                }

                await _userRoleRepo.InsertAsync(new UserRole {
                    RoleId = userModel.RoleId,
                    UserId= userEntity.UserId,
                });

                await transaction.SaveChangesAsync();
            }
        }

        public async Task DeleteOrganizationAsync(int OrganizationId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _orgRepo = transaction.GetRepository<Organization>();
                _userRoleRepo = transaction.GetRepository<UserRole>();
                _roleRouteMappingRepo = transaction.GetRepository<RoleRouteMapping>();

                // Admin priviledge check
                if (!await this.HasAdminPriviledge(_roleRepo, OrganizationId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                // Delete child data records first
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
                orgEntity = await _orgRepo.GetAsync(OrganizationId);
            }
            if (orgEntity == null) throw new ArgumentException($"Organization with identifier {OrganizationId} was not found");
            return Mapper.Map<OrganizationModel>(orgEntity);
        }

        public async Task<IEnumerable<OrganizationModel>> GetOrganizationsAsync()
        {
            List<OrganizationModel> organizationModels = new List<OrganizationModel>();

            using (var transaction = UnitOfWorkManager.Begin())
            {
                _userRoleRepo = transaction.GetRepository<UserRole>();

                return await _userRoleRepo.UnTrackableQuery().Where(x => x.Role.IsAdmin && x.UserId == User.UserId)
                    .Select(x => new OrganizationModel {
                        OrganizationId = x.Role.Organization.OrganizationId,
                        OrganizationName = x.Role.Organization.OrganizationName
                    }).ToListAsync();
            }
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
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _orgRepo = transaction.GetRepository<Organization>();

                if (!await this.HasAdminPriviledge(_roleRepo, organizationModel.OrganizationId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                Organization? orgEntity = await _orgRepo.GetAsync(organizationModel.OrganizationId);
                if (orgEntity == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);

                orgEntity = Mapper.Map(organizationModel, orgEntity);

                orgEntity = _orgRepo.Update(orgEntity);
                await transaction.SaveChangesAsync();
    
                return Mapper.Map<OrganizationModel>(orgEntity);
            }
        }

        public async Task<PagedModel<UserModel>> GetUserByOrganizationId(PagedModel<UserModel> pagedModel, int orgId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _roleRepo = transaction.GetRepository<Role>();
                _userRoleRepo = transaction.GetRepository<UserRole>();

                if (!await this.HasAdminPriviledge(_roleRepo, orgId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);


                int[] userRoleMappingIds = await _roleRepo.UnTrackableQuery().Where(x => x.OrganizationId == orgId).Select(x => x.RoleId).ToArrayAsync();
                if (string.IsNullOrEmpty(pagedModel.SearchString))
                {
                    pagedModel.SourceData = await _userRoleRepo.UnTrackableQuery()
                        .Where(x => userRoleMappingIds.Contains(x.RoleId))
                        .Skip(pagedModel.Skip)
                        .Take(pagedModel.PageSize)
                        .Select(u => new UserModel
                        {
                            FirstName = u.User.FirstName,
                            LastName = u.User.LastName,
                            UserName = u.User.UserName,
                            RoleId = u.RoleId,
                            Email = u.User.Email,
                            RoleName = u.Role.RoleName,
                        }).ToListAsync();

                    pagedModel.TotalItems = await _userRoleRepo.UnTrackableQuery().Where(x => userRoleMappingIds.Contains(x.RoleId)).CountAsync();
                }
                else
                {

                    pagedModel.SourceData = await _userRoleRepo.UnTrackableQuery()
                        .Where(x =>
                            userRoleMappingIds.Contains(x.RoleId) && (
                                x.User.FirstName.ToLower().Contains(pagedModel.SearchString.ToLower())
                                || x.User.LastName.ToLower().Contains(pagedModel.SearchString.ToLower())
                                || x.User.UserName.ToLower().Contains(pagedModel.SearchString.ToLower())
                            )
                        )
                        .Skip(pagedModel.Skip)
                        .Take(pagedModel.PageSize)
                        .Select(u => new UserModel
                        {
                            FirstName = u.User.FirstName,
                            LastName = u.User.LastName,
                            UserName = u.User.UserName,
                            RoleId = u.RoleId,
                            Email = u.User.Email,
                            RoleName = u.Role.RoleName,
                        }).ToListAsync();

                    pagedModel.TotalItems = await _userRoleRepo.UnTrackableQuery()
                        .Where(x =>
                            userRoleMappingIds.Contains(x.RoleId) && (
                                x.User.FirstName.ToLower().Contains(pagedModel.SearchString.ToLower())
                                || x.User.LastName.ToLower().Contains(pagedModel.SearchString.ToLower())
                                || x.User.UserName.ToLower().Contains(pagedModel.SearchString.ToLower())
                            )
                        )
                        .CountAsync();
                }

                return pagedModel;
            }
        }
    }
}
