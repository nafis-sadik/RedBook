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
    public class PolicyService : ServiceBase, IPolicyService
    {
        private readonly IRepositoryBase<Policy> _policyRepo;
        public PolicyService(
            ILogger<PolicyService> logger,
            IObjectMapper mapper,
            IUnitOfWork unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        {
            _policyRepo = unitOfWork.GetRepository<Policy>();
        }

        public async Task<PolicyModel> AddPolicyAsync(PolicyModel policyModel)
        {
            var requestUserRoleId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value);

            if (requestUserRoleId != CommonConstants.GenericRoles.SystemAdminRoleId && requestUserRoleId != CommonConstants.GenericRoles.AdminRoleId)
                throw new ArgumentException($"Only admin user can add new user to his organization");

            Policy policyEntity;
            using(var transaction = UnitOfWorkManager.Begin())
            {
                policyEntity = Mapper.Map<Policy>(policyModel);
                policyEntity = await _policyRepo.InsertAsync(policyEntity);
                await transaction.SaveChangesAsync();
            }

            return Mapper.Map<PolicyModel>(policyModel);
        }
        public Task DeletePolicyAsync(int PolicyId) => throw new NotImplementedException();
        public Task<PolicyModel> GetPolicyAsync(int PolicyId) => throw new NotImplementedException();
        public PagedModel<PolicyModel> GetPolicysAsync(PagedModel<PolicyModel> pagedPolicyModel) => throw new NotImplementedException();
        public Task<PolicyModel> UpdatePolicyAsync(PolicyModel Policy) => throw new NotImplementedException();
    }
}
