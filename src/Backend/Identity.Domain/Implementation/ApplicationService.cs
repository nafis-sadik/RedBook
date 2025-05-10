using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Domain;
using RedBook.Core.Models;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Identity.Domain.Implementation
{
    public class ApplicationService : ServiceBase, IApplicationService
    {
        private IRepositoryFactory _repositoryFactory;
        public ApplicationService(
            ILogger<ApplicationService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor)
        { }

        public async Task<ApplicationInfoModel> AddApplicationAsync(ApplicationInfoModel applicationModel)
        {
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                // Permission Check
                var _userRoleMappingRepo = _repositoryFactory.GetRepository<UserRoleMapping>();
                if (!await this.HasSystemAdminPriviledge(_userRoleMappingRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                // Insertion Operation
                var _appRepo = _repositoryFactory.GetRepository<Application>();
                Application applicationEntity = await _appRepo.InsertAsync(Mapper.Map<Application>(applicationModel));
                await _repositoryFactory.SaveChangesAsync();

                return Mapper.Map<ApplicationInfoModel>(applicationEntity);
            }
        }

        public async Task<ApplicationInfoModel> GetApplicationAsync(int applicationId)
        {
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                // Permission Check
                var _userRoleMappingRepo = _repositoryFactory.GetRepository<UserRoleMapping>();
                if (!await this.HasSystemAdminPriviledge(_userRoleMappingRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                // Read Operation
                var _appRepo = _repositoryFactory.GetRepository<Application>();
                Application? applicationEntity = await _appRepo.GetAsync(applicationId);

                if (applicationEntity == null) throw new ArgumentException($"Application {applicationId} not found");

                return Mapper.Map<ApplicationInfoModel>(applicationEntity);
            }
        }

        public async Task<ApplicationInfoModel> UpdateApplicationAsync(ApplicationInfoModel applicationModel)
        {
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                // Permission Check
                var _userRoleMappingRepo = _repositoryFactory.GetRepository<UserRoleMapping>();
                if (!await this.HasSystemAdminPriviledge(_userRoleMappingRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                // Update Operation
                var _appRepo = _repositoryFactory.GetRepository<Application>();
                Application? applicationEntity = await _appRepo.GetAsync(applicationModel.Id);
                if (applicationEntity == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);

                applicationEntity = Mapper.Map(applicationModel, applicationEntity);
                applicationEntity = _appRepo.Update(applicationEntity);
                await _repositoryFactory.SaveChangesAsync();

                return Mapper.Map<ApplicationInfoModel>(applicationEntity);
            }
        }

        public async Task DeleteApplicationAsync(int applicationId)
        {
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _appRepo = _repositoryFactory.GetRepository<Application>();

                await _appRepo.DeleteAsync(applicationId);

                await _repositoryFactory.SaveChangesAsync();
            }
        }

        public async Task<PagedModel<ApplicationInfoModel>> GetApplicationsAsync(PagedModel<ApplicationInfoModel> applicationModel)
        {
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _appRepo = _repositoryFactory.GetRepository<Application>();
                var _userRoleRepo = _repositoryFactory.GetRepository<UserRoleMapping>();

                if (!await this.HasSystemAdminPriviledge(_userRoleRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                applicationModel.SourceData = await _appRepo
                    .UnTrackableQuery()
                    .Where(x => x.ApplicationId > 0)
                    .Skip(applicationModel.Skip)
                    .Take(applicationModel.PageLength)
                    .Select(x => new ApplicationInfoModel
                    {
                        Id = x.ApplicationId,
                        ApplicationName = x.ApplicationName
                    })
                    .ToListAsync();
            }

            return applicationModel;
        }

        public async Task<IEnumerable<ApplicationInfoModel>> GetAllApplicationAsync()
        {
            List<ApplicationInfoModel> applicationModelCollection = new List<ApplicationInfoModel>();

            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _appRepo = _repositoryFactory.GetRepository<Application>();
                var _userRoleRepo = _repositoryFactory.GetRepository<UserRoleMapping>();

                if (!await this.HasSystemAdminPriviledge(_userRoleRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                applicationModelCollection = await _appRepo
                    .UnTrackableQuery()
                    .Where(x => x.ApplicationId > 0)
                    .Select(x => new ApplicationInfoModel
                    {
                        Id = x.ApplicationId,
                        ApplicationName = x.ApplicationName,
                    }).ToListAsync();
            }

            return applicationModelCollection;
        }
    }
}