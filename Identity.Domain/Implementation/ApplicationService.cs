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
    public class ApplicationService : ServiceBase, IApplicationService
    {
        private IRepositoryBase<Application> _appRepository;
        public ApplicationService(
            ILogger<ApplicationService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        {
            var userRoleId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value);
            if (userRoleId != CommonConstants.GenericRoles.SystemAdminRoleId)
                throw new ArgumentException($"Only System Admin users of Blume Digital Corp. have access to execute this operation");
        }

        public async Task<ApplicationInfoModel> AddApplicationAsync(ApplicationInfoModel applicationModel)
        {
            Application applicationEntity = Mapper.Map<Application>(applicationModel);
            using(var transaction = UnitOfWorkManager.Begin())
            {
                _appRepository = transaction.GetRepository<Application>();
                applicationEntity = await _appRepository.InsertAsync(new Application {
                    ApplicationName = applicationModel.ApplicationName,
                    OrganizationId = applicationModel.OrganizationId
                });
                await transaction.SaveChangesAsync();
            }

            return Mapper.Map<ApplicationInfoModel>(applicationEntity);
        }

        public async Task<ApplicationInfoModel> GetApplicationAsync(int applicationId)
        {
            Application? applicationEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _appRepository = transaction.GetRepository<Application>();
                applicationEntity = await _appRepository.GetByIdAsync(applicationId);
            }

            if (applicationEntity == null)
                throw new ArgumentException($"Application {applicationId} not found");

            return Mapper.Map<ApplicationInfoModel>(applicationEntity);
        }

        public async Task<ApplicationInfoModel> UpdateApplicationAsync(ApplicationInfoModel applicationModel)
        {
            Application? applicationEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _appRepository = transaction.GetRepository<Application>();
                applicationEntity = await _appRepository.GetByIdAsync(applicationModel.Id);
                applicationEntity = Mapper.Map(applicationModel, applicationEntity);
                if(applicationEntity == null) throw new ArgumentException($"Application {applicationModel.Id} not found");
                applicationEntity = _appRepository.Update(applicationEntity);
                await transaction.SaveChangesAsync();
            }
            return Mapper.Map<ApplicationInfoModel>(applicationEntity);
        }

        public async Task DeleteApplicationAsync(int applicationId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                await _appRepository.DeleteAsync(applicationId);
                await transaction.SaveChangesAsync();
            }
        }

        public async Task<PagedModel<ApplicationInfoModel>> GetApplicationsAsync(PagedModel<ApplicationInfoModel> applicationModel)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _appRepository = transaction.GetRepository<Application>();
                applicationModel.SourceData = await _appRepository
                    .UnTrackableQuery()
                    .Where(x => x.ApplicationId > 0)
                    .Skip(applicationModel.Skip)
                    .Take(applicationModel.PageSize)
                    .Select(x => new ApplicationInfoModel
                    {
                        Id = x.ApplicationId,
                        ApplicationName = x.ApplicationName
                    })
                    .ToListAsync();
            }

            return applicationModel;
        }
    }
}