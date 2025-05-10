﻿using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Models;

namespace Identity.WebAPI.Controllers
{
    /// <summary>
    /// Route Controller for working with routes
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController(IRouteServices routeServices) : ControllerBase
    {
        private readonly IRouteServices _routeServices = routeServices;

        /// <summary>
        /// Paginated list of application routes (System Admin User Only)
        /// </summary>
        /// <param name="pagedRouteCollection">An implementation of IApplicationService injected by IOC Controller</param>
        [HttpGet]
        [Route("GetPaged")]
        public async Task<IActionResult> GetPaged([FromQuery] PagedModel<RouteModel> pagedRouteCollection) => Ok(await _routeServices.GetPagedRoutes(pagedRouteCollection));

        /// <summary>
        /// Returns all routes of specified application
        /// </summary>
        [HttpGet]
        [Route("App")]
        public async Task<IActionResult> GetAll() => Ok(await _routeServices.GetAppRoutes());

        /// <summary>
        /// Returns allowed menu routes of requesting user
        /// </summary>
        [HttpGet]
        [Route("Menu")]
        public async Task<IActionResult> GetMenu() => Ok(await _routeServices.GetAppMenuRoutes());

        /// <summary>
        /// Returns detailed info of route identified by id provided
        /// </summary>
        /// <param name="routeId">User unique identifier<see cref="int"/>.</param>
        [HttpGet]
        [Route("GetDetails/{routeId}")]
        public async Task<IActionResult> Get(int routeId) => Ok(await _routeServices.GetRoute(routeId));

        /// <summary>
        /// Returns collection of Routes allowed to dole by role id
        /// </summary>
        /// <param name="roleId">User unique identifier<see cref="int"/>.</param>
        [HttpGet]
        [Route("Role/{roleId}")]
        public async Task<IActionResult> GetRoutesByRoleId(int roleId) => Ok(await _routeServices.GetRoutesByRoleId(roleId));

        /// <summary>
        /// Add new route to specified application
        /// </summary>
        /// <param name="routeInfo">User unique identifier<see cref="RouteModel"/>.</param>
        [HttpPost]
        public async Task<IActionResult> Add(RouteModel routeInfo) => Ok(await _routeServices.AddRoute(routeInfo));

        /// <summary>
        /// Updates existing route information
        /// </summary>
        /// <param name="routeInfo">User unique identifier<see cref="RouteModel"/>.</param>
        [HttpPut]
        public async Task<IActionResult> Update(RouteModel routeInfo) => Ok(await _routeServices.UpdateRoute(routeInfo));

        [HttpGet]
        [Route("Types")]
        public async Task<IActionResult> GetAllRouteTypes() => Ok(await _routeServices.GetRouteTypes());

        /// <summary>
        /// Permanantly deletes route specified
        /// </summary>
        /// <param name="routeId">User unique identifier<see cref="int"/>.</param>
        [HttpDelete]
        [Route("{routeId}")]
        public async Task<IActionResult> Delete(int routeId)
        {
            await _routeServices.DeleteRoute(routeId);
            return Ok();
        }
    }
}
