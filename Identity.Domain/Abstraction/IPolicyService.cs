using Identity.Data.Models;
using RedBook.Core.Models;

namespace Identity.Domain.Abstraction
{
    public interface IPolicyService
    {
        Task<PolicyModel> AddPolicyAsync(PolicyModel Policy);
        Task<PolicyModel> GetPolicyAsync(int PolicyId);
        PagedModel<PolicyModel> GetPolicysAsync(PagedModel<PolicyModel> pagedPolicyModel);
        Task<PolicyModel> UpdatePolicyAsync(PolicyModel Policy);
        Task DeletePolicyAsync(int PolicyId);
    }
}
