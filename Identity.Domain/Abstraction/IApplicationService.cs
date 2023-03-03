using Identity.Data.Models;

namespace Identity.Domain.Abstraction
{
    public interface IApplicationService
    {
        Task<ApplicationInfoModel> AddApplication(ApplicationInfoModel applicationModel);
        Task<ApplicationInfoModel> UpdateApplication(ApplicationInfoModel applicationModel);
        Task DeleteApplication(int applicationId);
        Task<ApplicationInfoModel> GetApplication(int applicationId);
    }
}
