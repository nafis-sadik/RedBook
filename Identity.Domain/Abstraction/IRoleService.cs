using Identity.Data.Models;

namespace Identity.Domain.Abstraction
{
    public interface IRoleService
    {
        Task<RoleModel> AddRoleAsync(RoleModel role);

        Task<RoleModel> UpdateRoleAsync(RoleModel role);

        Task<IEnumerable<RoleModel>> GetOrganizationRoles(int orgId);

        Task<int[]?> GetOrganizationsAllowedToUserByRoute(int userId, int routeId);

        Task DeleteRoleAsync(int roleId);

        Task InvertRouteRoleMapping(int roleId, int routeId);
    }
}
