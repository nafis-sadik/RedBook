using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Domain;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using System.Security.Claims;

namespace Identity.Domain.Implementation
{
    public class ApplicationService : ServiceBase, IApplicationService
    {
        private readonly IRepositoryBase<Application> _appRepository;
        public ApplicationService(
            ILogger<ApplicationService> logger,
            IObjectMapper mapper,
            IUnitOfWork unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        {
            var userRoleId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value);
            if (userRoleId != CommonConstants.GenericRoles.SystemAdminRoleId)
                throw new ArgumentException($"Only System Admin users have access to execute this operation");

            _appRepository = unitOfWork.GetRepository<Application>();
        }
        public async Task<ApplicationInfoModel> AddApplication(ApplicationInfoModel applicationModel)
        {
            var applicationEntity = Mapper.Map<Application>(applicationModel);
            using(var transaction = UnitOfWorkManager.Begin())
            {
                applicationEntity = await _appRepository.InsertAsync(applicationEntity);
                await transaction.SaveChangesAsync();
            }
            return Mapper.Map<ApplicationInfoModel>(applicationEntity);
        }
        public async Task<ApplicationInfoModel> GetApplication(int applicationId)
        {
            var applicationEntity = await _appRepository.GetByIdAsync(applicationId);
            return Mapper.Map<ApplicationInfoModel>(applicationEntity);
        }
        public async Task<ApplicationInfoModel> UpdateApplication(ApplicationInfoModel applicationModel)
        {
            Application applicationEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                applicationEntity = await _appRepository.GetByIdAsync(applicationModel.Id);
                applicationEntity = Mapper.Map(applicationModel, applicationEntity);
                applicationEntity = _appRepository.Update(applicationEntity);
                await transaction.SaveChangesAsync();
            }
            return Mapper.Map<ApplicationInfoModel>(applicationEntity);
        }
        public async Task DeleteApplication(int applicationId)
        {
            Application applicationEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                applicationEntity = await _appRepository.GetByIdAsync(applicationId);
                applicationEntity = await _appRepository.DeleteAsync(applicationEntity);
                await transaction.SaveChangesAsync();
            }
        }
    }
}