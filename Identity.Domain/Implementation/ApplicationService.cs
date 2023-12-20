﻿using Identity.Data.Entities;
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
    public class ApplicationService : ServiceBase, IApplicationService
    {
        private IRepositoryBase<Role> _roleRepo;
        private IRepositoryBase<Application> _appRepo;
        public ApplicationService(
            ILogger<ApplicationService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        { }

        public async Task<ApplicationInfoModel> AddApplicationAsync(ApplicationInfoModel applicationModel)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _appRepo = transaction.GetRepository<Application>();
                _roleRepo = transaction.GetRepository<Role>();

                if (!await this.HasSystemAdminPriviledge(_roleRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                Application applicationEntity = await _appRepo.InsertAsync(Mapper.Map<Application>(applicationModel));
                await transaction.SaveChangesAsync();

                return Mapper.Map<ApplicationInfoModel>(applicationEntity);
            }
        }

        public async Task<ApplicationInfoModel> GetApplicationAsync(int applicationId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _appRepo = transaction.GetRepository<Application>();
                Application? applicationEntity = await _appRepo.GetAsync(applicationId);

                if (applicationEntity == null)
                    throw new ArgumentException($"Application {applicationId} not found");

                return Mapper.Map<ApplicationInfoModel>(applicationEntity);
            }
        }

        public async Task<ApplicationInfoModel> UpdateApplicationAsync(ApplicationInfoModel applicationModel)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _appRepo = transaction.GetRepository<Application>();
                _roleRepo = transaction.GetRepository<Role>();

                if (!await this.HasSystemAdminPriviledge(_roleRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                Application? applicationEntity = await _appRepo.GetAsync(applicationModel.Id);
                if(applicationEntity == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);

                applicationEntity = Mapper.Map(applicationModel, applicationEntity);
                applicationEntity = _appRepo.Update(applicationEntity);
                await transaction.SaveChangesAsync();

                return Mapper.Map<ApplicationInfoModel>(applicationEntity);
            }
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
                _roleRepo = transaction.GetRepository<Role>();

                if (!await this.HasSystemAdminPriviledge(_roleRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                applicationModel.SourceData = await _appRepo
                    .UnTrackableQuery()
                    .Where(x => x.ApplicationId > 0)
                    .Skip(applicationModel.Skip)
                    .Take(applicationModel.PageLength)
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
                _roleRepo = transaction.GetRepository<Role>();

                if (!await this.HasSystemAdminPriviledge(_roleRepo)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                applicationModelCollection = await _appRepo
                    .UnTrackableQuery()
                    .Where(x => x.ApplicationId > 0)
                    .Select(x => new ApplicationInfoModel
                    {
                        Id = x.ApplicationId,
                        ApplicationName = x.ApplicationName,
                        OrganizationId = x.OrganizationId,
                    }).ToListAsync();
            }

            return applicationModelCollection;
        }
    }
}