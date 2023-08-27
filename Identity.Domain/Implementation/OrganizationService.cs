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
            Organization orgEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                orgEntity = Mapper.Map<Organization>(organizationModel);
                _orgRepo = transaction.GetRepository<Organization>();
                orgEntity = await _orgRepo.InsertAsync(new Organization {
                    OrganizationName = organizationModel.OrganizationName,
                });
                await transaction.SaveChangesAsync();
            }
            return Mapper.Map<OrganizationModel>(orgEntity);
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

        public async Task<PagedModel<OrganizationModel>> GetOrganizationsAsync(PagedModel<OrganizationModel> pagedOrganizationModel)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _orgRepo = transaction.GetRepository<Organization>();
                pagedOrganizationModel.SourceData = await _orgRepo.UnTrackableQuery()
                    .Where(x => x.OrganizationId > 0)
                    .Skip(pagedOrganizationModel.Skip)
                    .Take(pagedOrganizationModel.PageSize)
                    .Select(x => new OrganizationModel {
                        Id = x.OrganizationId,
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
                orgEntity = await _orgRepo.GetByIdAsync(organizationModel.Id);
                if (orgEntity == null) throw new ArgumentException($"Organization with identifier {organizationModel.Id} was not found");
                orgEntity = Mapper.Map(organizationModel, orgEntity);
                orgEntity = _orgRepo.Update(orgEntity);
                await transaction.SaveChangesAsync();
            }
            return Mapper.Map<OrganizationModel>(orgEntity);
        }
    }
}
