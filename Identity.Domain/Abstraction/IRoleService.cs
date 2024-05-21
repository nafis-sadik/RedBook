using Identity.Data.Models;

namespace Identity.Domain.Abstraction
{
    public interface IRoleService
    {
        Task<RoleModel> AddRoleAsync(RoleModel role);

        Task<RoleModel> UpdateRoleAsync(RoleModel role);

        Task<IEnumerable<RoleModel>> GetOrganizationRoles(int orgId);


        /// <summary>
        /// Gets the organizations that the user is allowed to access for the specified route.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="routeId">The ID of the route.</param>
        /// <returns>An array of organization IDs that the user is allowed to access for the specified route.</returns>
        Task<int[]?> GetOrganizationsAllowedToUserByRoute(int userId, int routeId);

        Task DeleteRoleAsync(int roleId);

        Task InvertRouteRoleMapping(int roleId, int routeId);
    }
}
