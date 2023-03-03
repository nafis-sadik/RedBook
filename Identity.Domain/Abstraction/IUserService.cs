using Identity.Data.Models;

namespace Identity.Domain.Abstraction
{
    public interface IUserService
    {
        // Public API
        Task<string?> SignUpAsync(UserModel userModel);
        Task<string?> LogInAsync(UserModel userModel);

        // Organization Admin API
        Task<UserModel> RegisterNewUser(UserModel userModel);
        Task<bool> ResetPassword(string userId);

        // All User API
        Task<UserModel> UpdateOwnInformation(UserModel userModel);
        Task<UserModel?> GetOwnInformation();
        Task<bool> ArchiveAccount();

        // System Admin API
        Task<bool> DeleteAccount(string userId);
        Task<bool> UnArchiveAccount(string userId);
        Task<UserModel> RegisterAdminUser(UserModel userModel);
    }
}
