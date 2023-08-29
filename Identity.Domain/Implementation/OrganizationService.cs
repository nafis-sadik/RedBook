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
        private IRepositoryBase<Organization> _orgRepo;
        private IRepositoryBase<Role> _roleRepo;

        public OrganizationService(
            ILogger<ApplicationService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        { }

        public async Task<OrganizationModel> AddOrganizationAsync(OrganizationModel organizationModel)
        {
            Organization? orgEntity = null;

            string? requesterRoleIdStr = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;
            if (string.IsNullOrEmpty(requesterRoleIdStr) || !int.TryParse(requesterRoleIdStr, out int requesterRoleId))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            string? requesterUserId = User?.Claims.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value;
            if (string.IsNullOrEmpty(requesterUserId))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            using (var transaction = UnitOfWorkManager.Begin())
            {
                _orgRepo = transaction.GetRepository<Organization>();
                _roleRepo = transaction.GetRepository<Role>();

                // Get role to check permissions
                Role? requesterRole = await _roleRepo.GetByIdAsync(requesterRoleId);
                if (requesterRole == null)
                    throw new ArgumentException($"Requester role with identifier {requesterRoleId} was not found");

                // A retailer can add new organization to onboard new customer
                // An admin user can add new organization to manage multiple businesses from redbook
                if(requesterRole.IsAdmin || requesterRole.IsRetailer)
                {
                    orgEntity = Mapper.Map<Organization>(organizationModel);
                    orgEntity.OrganizationId = 0;
                    orgEntity.CreateDate = DateTime.UtcNow;
                    orgEntity.CreatedBy = requesterUserId;
                    orgEntity = await _orgRepo.InsertAsync(orgEntity);
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
            Organization? orgEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _orgRepo = transaction.GetRepository<Organization>();
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
                orgEntity = await _orgRepo.GetByIdAsync(OrganizationId);
            }
            if (orgEntity == null) throw new ArgumentException($"Organization with identifier {OrganizationId} was not found");
            return Mapper.Map<OrganizationModel>(orgEntity);
        }

        public async Task<IEnumerable<OrganizationModel>> GetOrganizationsAsync()
        {
            List<OrganizationModel> organizationModels = new List<OrganizationModel>();
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _orgRepo = transaction.GetRepository<Organization>();
                organizationModels = await _orgRepo.UnTrackableQuery()
                    .Where(x => x.OrganizationId > 0)
                    .Select(x => new OrganizationModel
                    {
                        OrganizationId = x.OrganizationId,
                        OrganizationName = x.OrganizationName
                    }).ToListAsync();
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
                orgEntity = await _orgRepo.GetByIdAsync(organizationModel.OrganizationId);
                if (orgEntity == null) throw new ArgumentException($"Organization with identifier {organizationModel.OrganizationId} was not found");
                orgEntity = Mapper.Map(organizationModel, orgEntity);
                orgEntity = _orgRepo.Update(orgEntity);
                await transaction.SaveChangesAsync();
            }
            return Mapper.Map<OrganizationModel>(orgEntity);
        }
    }
}
