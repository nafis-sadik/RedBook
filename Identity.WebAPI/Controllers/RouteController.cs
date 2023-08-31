using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Models;

namespace Identity.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly IRouteServices _routeServices;
        public RouteController(IRouteServices routeService)
        {
            _routeServices = routeService;
        }

        /// <summary>
        /// Paginated list of application routes (System Admin User Only)
        /// </summary>
        /// <param name="pagedRouteCollection">An implementation of IApplicationService injected by IOC Controller</param>
        [HttpGet]
        [Route("GetPaged")]
        public async Task<IActionResult> GetPaged([FromQuery] PagedModel<RouteModel> pagedRouteCollection) => Ok(await _routeServices.GetPagedRoutes(pagedRouteCollection));

        [HttpGet]
        [Route("GetAll/{userId}")]
        public async Task<IActionResult> GetAll(string userId) => Ok(await _routeServices.GetAllRoutes(userId));

        [HttpGet]
        public async Task<IActionResult> Get(int routeId) => Ok(await _routeServices.GetRoute(routeId));

        /// <summary>
        /// Add new route to specified application
        /// </summary>
        /// <param name="routeInfo">User unique identifier<see cref="RouteModel"/>.</param>
        [HttpPost]
        public async Task<IActionResult> Add(RouteModel routeInfo) => Ok(await _routeServices.AddRoute(routeInfo));

        [HttpPut]
        public async Task<IActionResult> Update(RouteModel routeInfo) => Ok(await _routeServices.UpdateRoute(routeInfo));

        [HttpDelete]
        public async Task<IActionResult> Delete(int routeId)
        {
            await _routeServices.DeleteRoute(routeId);
            return Ok();
        }
    }
}
