using Identity.Data.Models;
using RedBook.Core.Models;

namespace Identity.Domain.Abstraction
{
    public interface IApplicationService
    {
        Task<ApplicationInfoModel> AddApplicationAsync(ApplicationInfoModel applicationModel);
        Task<ApplicationInfoModel> UpdateApplicationAsync(ApplicationInfoModel applicationModel);
        Task DeleteApplicationAsync(int applicationId);
        Task<ApplicationInfoModel> GetApplicationAsync(int applicationId);
        Task<IEnumerable<ApplicationInfoModel>> GetAllApplicationAsync();
        Task<PagedModel<ApplicationInfoModel>> GetApplicationsAsync(PagedModel<ApplicationInfoModel> applicationModel);
    }
}
