﻿using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebAPI.Controllers
{
    /// <summary>
    /// Role Module
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController(IRoleService roleService) : ControllerBase
    {
        private readonly IRoleService _roleServices = roleService;

        [HttpPost]
        public async Task<IActionResult> AddRoleAsync(RoleModel roleModel) => Ok(await _roleServices.AddRoleAsync(roleModel));

        [HttpPut]
        public async Task<IActionResult> UpdateRoleAsync(RoleModel roleModel) => Ok(await _roleServices.UpdateRoleAsync(roleModel));

        /// <summary>
        /// Remove roles by Role Id
        /// </summary>
        [HttpDelete]
        [Route("{roleId}")]
        public async Task<IActionResult> RemoveRoleAsync(int roleId)
        {
            await _roleServices.DeleteRoleAsync(roleId);
            return Ok();
        }

        /// <summary>
        /// Returns roles by Organization Id
        /// </summary>
        [HttpGet]
        [Route("Organization/{orgId}")]
        public async Task<IActionResult> GetByOrg(int orgId) => Ok(await _roleServices.GetOrganizationRoles(orgId));

        /// <summary>
        /// Map role with route for permission
        /// </summary>
        [HttpGet]
        [Route("/AllowRouteForRole/{roleId}/{routeId}")]
        public async Task<IActionResult> MapRoleRoute(int roleId, int routeId) => Ok(await _roleServices.InvertRouteRoleMapping(roleId, routeId));

        /// <summary>
        /// Map role with route for permission
        /// </summary>
        [HttpGet]
        [Route("/GetAllowedOrganizationsToUserByRoute/{userId}/{routeId}")]
        public async Task<IActionResult> GetOrganizationsAllowedToUserByRoute(int userId, int routeId) => Ok(await _roleServices.GetOrganizationsAllowedToUserByRoute(userId, routeId));
    }
}
