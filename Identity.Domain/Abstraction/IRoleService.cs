using Identity.Data.Models;
using RedBook.Core.Models;

namespace Identity.Domain.Abstraction
{
    public interface IRoleService
    {
        Task<RoleModel> AddRoleAsync(RoleModel role);
        Task<RoleModel> GetRoleAsync(int roleId);
        Task<PagedModel<RoleModel>> GetRolesAsync(PagedModel<RoleModel> pagedRoleModel);
        Task<RoleModel> UpdateRoleAsync(RoleModel role);
        Task DeleteRoleAsync(int roleId);
    }
}
