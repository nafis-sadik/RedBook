using Identity.Data.Models;
using RedBook.Core.Models;

namespace Identity.Domain.Abstraction
{
    public interface IRoleService
    {
        Task<RoleModel> AddRoleAsync(RoleModel role);
        Task<PagedModel<RoleModel>> GetRolesAsync();
        Task<RoleModel> UpdateRoleAsync(RoleModel role);
        Task<IEnumerable<RoleModel>> GetOrganizationRoles(int orgId);
        Task DeleteRoleAsync(int roleId);
    }
}
