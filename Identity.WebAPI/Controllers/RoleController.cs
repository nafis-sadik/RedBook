using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Identity.Domain.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Models;

namespace Identity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleServices;

        public RoleController(IRoleService roleService)
        {
            _roleServices = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleAsync(RoleModel roleModel) => Ok(await _roleServices.AddRoleAsync(roleModel));

        [HttpPut]
        public async Task<IActionResult> UpdateRoleAsync(RoleModel roleModel) => Ok(await _roleServices.UpdateRoleAsync(roleModel));

        [HttpDelete]
        [Route("{roleId}")]
        public async Task<IActionResult> RemoveRoleAsync(int roleId)
        {
            await _roleServices.DeleteRoleAsync(roleId);
            return Ok();
        }

        //[HttpGet]
        //public async Task<IActionResult> GetRoleAsync(int roleId) => Ok(await _roleServices.GetRoleAsync(roleId));

        /// <summary>
        /// Returns roles by Organization Id
        /// </summary>
        [HttpGet]
        [Route("OrganizationRoles/{orgId}")]
        public async Task<IActionResult> GetByOrg(int orgId) => Ok(await _roleServices.GetOrganizationRoles(orgId));

        /// <summary>
        /// Map role with route for permission
        /// </summary>
        [HttpGet]
        [Route("/AllowRouteForRole/{roleId}/{routeId}")]
        public async Task<IActionResult> MapRoleRoute(int roleId, int routeId)
        {
            await _roleServices.AllowRouteForRole(roleId, routeId);
            return Ok();
        }
    }
}
