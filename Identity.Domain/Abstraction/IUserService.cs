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
        Task<UserModel> RegisterNewUser(UserModel userModel);
        Task<UserModel> AddUserToBusiness(UserModel userModel);
        Task RemoveUserFromOrganization(int userId, int orgId);
        Task<PagedModel<UserModel>> GetUserByOrganizationId(PagedModel<UserModel> pagedModel, int orgId);
    }
}
