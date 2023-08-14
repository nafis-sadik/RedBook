using Identity.Data.Models;
using Identity.Domain.Abstraction;
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
        public async Task<IActionResult> RemoveRoleAsync(int roleId)
        {
            await _roleServices.DeleteRoleAsync(roleId);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetRoleAsync(int roleId) => Ok(await _roleServices.GetRoleAsync(roleId));

        [HttpGet]
        [Route("/PagedRoles")]
        public async Task<IActionResult> GetPagedAsync([FromQuery]PagedModel<RoleModel> pagedRoleModel) => Ok(await _roleServices.GetRolesAsync(pagedRoleModel));
    }
}
