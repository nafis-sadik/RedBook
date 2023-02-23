using Identity.Data.Models;

namespace Identity.Domain.Abstraction
{
    public interface IUserService
    {
        // Public API
        Task<string?> SignUpAsync(UserModel userModel);
        Task<string?> LogInAsync(UserModel userModel);

        // All User API
        Task<UserModel> UpdateOwnInformation(UserModel userModel);
        Task<UserModel> GetOwnInformation(string userId);
        Task<bool> ArchiveOwnId(string userId);

        // Organization Admin API
        Task<UserModel> RegisterNewUser(UserModel userModel);
        Task<bool> ArchiveAccount(string userId);
        Task<bool> ResetPassword(string userId);

        // System Admin API
        Task<bool> DeleteAccount(string userId);
    }
}
