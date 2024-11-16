using Identity.Data.Models;
using RedBook.Core.Models;

namespace Identity.Domain.Abstraction
{
    public interface IUserService
    {
        // Public API
        Task<string> LogInAsync(UserModel userModel);

        // User API
        Task<UserModel> UpdateAsync(UserModel userModel);
        Task<UserModel> GetById(int userId);
        Task ResetPassword(int userId);
        Task<List<OrganizationModel>> GetUserOrganizationsAsync();

        // Admin Only API
        Task DeleteAccount(int userId);
        Task<bool> ArchiveAccount(int userId);
        Task UnArchiveAccount(int userId);

        // Business Management
        Task<PagedModel<UserModel>> GetUserByOrganizationId(PagedModel<UserModel> pagedModel, int orgId);
    }
}
