using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Models;

namespace Identity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleServices;

        public RoleController(IRoleService roleService)
        {
            _roleServices = roleService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRoleAsync(RoleModel roleModel) => Ok(await _roleServices.AddRoleAsync(roleModel));

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateRoleAsync(RoleModel roleModel) => Ok(await _roleServices.UpdateRoleAsync(roleModel));

        [HttpDelete]
        [Authorize]
        [Route("/{roleId}")]
        public async Task<IActionResult> RemoveRoleAsync(int roleId) => Ok(_roleServices.DeleteRoleAsync(roleId));

        [HttpGet]
        [Authorize]
        [Route("/{roleId}")]
        public async Task<IActionResult> GetRoleAsync(int roleId) => Ok(_roleServices.GetRoleAsync(roleId));

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRoleListAsync(PagedModel<RoleModel> pagedRoleModel) => Ok(_roleServices.GetRolesAsync(pagedRoleModel));
    }
}
