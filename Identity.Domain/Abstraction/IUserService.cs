using Identity.Data.Models;

namespace Identity.Domain.Abstraction
{
    public interface IUserService
    {
        // Public API
        Task<string?> LogInAsync(UserModel userModel);

        // User API
        Task<UserModel> UpdateAsync(UserModel userModel);
        Task<UserModel?> GetById(string userId);
        Task<bool> ArchiveAccount(string userId);
        Task ResetPassword(string userId);

        // Admin Only API
        Task DeleteAccount(string userId);
        Task UnArchiveAccount(string userId);
        Task<UserModel> RegisterNewUser(UserModel userModel);
    }
}
