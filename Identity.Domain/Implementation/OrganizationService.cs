using Identity.Data.CommonConstant;
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
        private IRepositoryBase<UserRoleMapping> _userRoleRepo;
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
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleRepo = factory.GetRepository<Role>();
                _orgRepo = factory.GetRepository<Organization>();
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                // Creating the new organization
                Organization orgEntity = Mapper.Map<Organization>(organizationModel);

                orgEntity.CreatedBy = User.UserId;
                orgEntity.CreateDate = DateTime.UtcNow;
                orgEntity = await _orgRepo.InsertAsync(orgEntity);
                await factory.SaveChangesAsync();

                // Creating admin role for the organization
                Role? adminRoleForNewOrg = await _roleRepo.InsertAsync(new Role
                {
                    IsAdmin = RoleConstants.OwnerAdmin.IsAdmin,
                    IsRetailer = RoleConstants.OwnerAdmin.IsRetailer,
                    IsOwner = RoleConstants.OwnerAdmin.IsOwner,
                    IsSystemAdmin = RoleConstants.OwnerAdmin.IsSystemAdmin,
                    RoleName = RoleConstants.OwnerAdmin.RoleName,
                    OrganizationId = orgEntity.OrganizationId,
                });
                await factory.SaveChangesAsync();

                // User role mapping, onboarding api shall take care of the retail user scinerio
                await _userRoleRepo.InsertAsync(new UserRoleMapping
                {
                    RoleId = adminRoleForNewOrg.RoleId,
                    UserId = User.UserId,
                });
                await factory.SaveChangesAsync();

                return Mapper.Map<OrganizationModel>(orgEntity);
            }
        }

        public async Task DeleteOrganizationAsync(int OrganizationId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _roleRepo = factory.GetRepository<Role>();
                _orgRepo = factory.GetRepository<Organization>();
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();
                _roleRouteMappingRepo = factory.GetRepository<RoleRouteMapping>();

                // Admin priviledge check
                if (!await this.HasOwnerAdminPriviledge(_userRoleRepo, OrganizationId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

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
                    await factory.SaveChangesAsync();

                    // Delete Role Route Mapping Records
                    int[]? roleRouteMappingIds = await _roleRouteMappingRepo.UnTrackableQuery().Where(x => x.RoleId == orgRoleId).Select(x => x.MappingId).ToArrayAsync();
                    foreach (int mappingId in roleRouteMappingIds)
                    {
                        await _roleRouteMappingRepo.DeleteAsync(mappingId);
                    }
                    await factory.SaveChangesAsync();

                    await _roleRepo.DeleteAsync(orgRoleId);
                    await factory.SaveChangesAsync();
                }

                await _orgRepo.DeleteAsync(OrganizationId);
                await factory.SaveChangesAsync();
            }
        }

        public async Task<OrganizationModel> GetOrganizationAsync(int OrganizationId)
        {
            Organization? orgEntity;
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _orgRepo = factory.GetRepository<Organization>();
                orgEntity = await _orgRepo.GetAsync(OrganizationId);
            }
            if (orgEntity == null) throw new ArgumentException($"Organization with identifier {OrganizationId} was not found");
            return Mapper.Map<OrganizationModel>(orgEntity);
        }

        public async Task<IEnumerable<OrganizationModel>> GetOrganizationsAsync()
        {
            List<OrganizationModel> organizationModels = new List<OrganizationModel>();

            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                return await _userRoleRepo.UnTrackableQuery().Where(x => (x.Role.IsAdmin == true || x.Role.IsOwner) && x.UserId == User.UserId)
                    .Select(x => new OrganizationModel {
                        OrganizationId = x.Role.Organization.OrganizationId,
                        OrganizationName = x.Role.Organization.OrganizationName
                    }).ToListAsync();
            }
        }

        public async Task<IEnumerable<OrganizationModel>> GetUserOrganizationsAsync()
        {
            using(var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                return await _userRoleRepo.UnTrackableQuery()
                    .Where(x => x.UserId == User.UserId)
                    .Select(mapping => new OrganizationModel
                    {
                        OrganizationId = mapping.Role.OrganizationId,
                        OrganizationName = mapping.Role.Organization.OrganizationName
                    })
                    .ToListAsync();
            }
        }

        public async Task<PagedModel<OrganizationModel>> GetPagedOrganizationsAsync(PagedModel<OrganizationModel> pagedOrganizationModel)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _orgRepo = factory.GetRepository<Organization>();

                pagedOrganizationModel.SourceData = await _orgRepo.UnTrackableQuery()
                    .Where(x => x.OrganizationId > 0)
                    .Skip(pagedOrganizationModel.Skip)
                    .Take(pagedOrganizationModel.PageLength)
                    .Select(x => new OrganizationModel {
                        OrganizationId = x.OrganizationId,
                        OrganizationName = x.OrganizationName
                    }).ToListAsync();
            }

            return pagedOrganizationModel;
        }

        public async Task<OrganizationModel> UpdateOrganizationAsync(OrganizationModel organizationModel)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();
                _orgRepo = factory.GetRepository<Organization>();

                if (!await this.HasAdminPriviledge(_userRoleRepo, organizationModel.OrganizationId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                Organization? orgEntity = await _orgRepo.GetAsync(organizationModel.OrganizationId);
                if (orgEntity == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);

                orgEntity = Mapper.Map(organizationModel, orgEntity);

                orgEntity = _orgRepo.Update(orgEntity);
                await factory.SaveChangesAsync();
    
                return Mapper.Map<OrganizationModel>(orgEntity);
            }
        }
    }
}
