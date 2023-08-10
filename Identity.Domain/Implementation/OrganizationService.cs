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
using System.Security.Claims;

namespace Identity.Domain.Implementation
{
    public class OrganizationService : ServiceBase, IOrganizationService
    {
        private readonly IRepositoryBase<Organization> _orgRepo;
        public OrganizationService(
            ILogger<ApplicationService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        {
            var userRoleId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value);
            if (userRoleId != CommonConstants.GenericRoles.SystemAdminRoleId)
                throw new ArgumentException($"Only System Admin users have access to execute this operation");
        }

        public async Task<OrganizationModel> AddOrganizationAsync(OrganizationModel organizationModel)
        {
            Organization orgEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                orgEntity = Mapper.Map<Organization>(organizationModel);
                orgEntity = await _orgRepo.InsertAsync(orgEntity);
                await transaction.SaveChangesAsync();
            }
            return Mapper.Map<OrganizationModel>(orgEntity);
        }
        public async Task DeleteOrganizationAsync(int OrganizationId)
        {
            Organization orgEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                orgEntity = await _orgRepo.GetByIdAsync(OrganizationId);
                await _orgRepo.DeleteAsync(orgEntity);
                await transaction.SaveChangesAsync();
            }
        }
        public async Task<OrganizationModel> GetOrganizationAsync(int OrganizationId)
        {
            Organization orgEntity = await _orgRepo.GetByIdAsync(OrganizationId);
            return Mapper.Map<OrganizationModel>(orgEntity);
        }
        public async Task<PagedModel<OrganizationModel>> GetOrganizationsAsync(PagedModel<OrganizationModel> pagedOrganizationModel)
        {
            PagedModel<Organization> pagedOrg= Mapper.Map<PagedModel<Organization>>(pagedOrganizationModel);
            pagedOrg = await _orgRepo.GetPagedAsync(pagedOrg);
            return Mapper.Map<PagedModel<OrganizationModel>>(pagedOrg);
        }
        public async Task<OrganizationModel> UpdateOrganizationAsync(OrganizationModel organizationModel)
        {
            Organization orgEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                orgEntity = Mapper.Map<Organization>(organizationModel);
                orgEntity = _orgRepo.Update(orgEntity);
                await transaction.SaveChangesAsync();
            }
            return Mapper.Map<OrganizationModel>(orgEntity);
        }
    }
}
