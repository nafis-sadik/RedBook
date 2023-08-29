using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
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
        private IRepositoryBase<Application> _appRepo;
        private IRepositoryBase<Role> _roleRepo;
        public ApplicationService(
            ILogger<ApplicationService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        { }

        public async Task<ApplicationInfoModel> AddApplicationAsync(ApplicationInfoModel applicationModel)
        {
            Application applicationEntity = Mapper.Map<Application>(applicationModel);
            using(var transaction = UnitOfWorkManager.Begin())
            {
                _appRepo = transaction.GetRepository<Application>();
                applicationEntity = await _appRepo.InsertAsync(new Application {
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
                _appRepo = transaction.GetRepository<Application>();
                applicationEntity = await _appRepo.GetByIdAsync(applicationId);
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
                _appRepo = transaction.GetRepository<Application>();
                applicationEntity = await _appRepo.GetByIdAsync(applicationModel.Id);
                applicationEntity = Mapper.Map(applicationModel, applicationEntity);
                if(applicationEntity == null) throw new ArgumentException($"Application {applicationModel.Id} not found");
                applicationEntity = _appRepo.Update(applicationEntity);
                await transaction.SaveChangesAsync();
            }
            return Mapper.Map<ApplicationInfoModel>(applicationEntity);
        }

        public async Task DeleteApplicationAsync(int applicationId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                await _appRepo.DeleteAsync(applicationId);
                await transaction.SaveChangesAsync();
            }
        }

        public async Task<PagedModel<ApplicationInfoModel>> GetApplicationsAsync(PagedModel<ApplicationInfoModel> applicationModel)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _appRepo = transaction.GetRepository<Application>();
                applicationModel.SourceData = await _appRepo
                    .UnTrackableQuery()
                    .Where(x => x.ApplicationId > 0)
                    .Skip(applicationModel.Skip)
                    .Take(applicationModel.PageSize)
                    .Select(x => new ApplicationInfoModel
                    {
                        Id = x.ApplicationId,
                        ApplicationName = x.ApplicationName,
                        OrganizationId = x.OrganizationId,
                    })
                    .ToListAsync();
            }

            return applicationModel;
        }

        public async Task<IEnumerable<ApplicationInfoModel>> GetAllApplicationAsync()
        {
            List<ApplicationInfoModel> applicationModelCollection = new List<ApplicationInfoModel>();

            using (var transaction = UnitOfWorkManager.Begin())
            {
                _appRepo = transaction.GetRepository<Application>();
                applicationModelCollection = await _appRepo
                    .UnTrackableQuery()
                    .Where(x => x.ApplicationId > 0)
                    .Select(x => new ApplicationInfoModel
                    {
                        Id = x.ApplicationId,
                        ApplicationName = x.ApplicationName
                    }).ToListAsync();
            }

            return applicationModelCollection;
        }
    }
}