using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RouteController : ControllerBase
    {
        private readonly IRouteServices _routeServices;
        public RouteController(IRouteServices routeService)
        {
            _routeServices = routeService;            
        }

        [HttpGet]
        public async Task<IActionResult> Get(int routeId) => Ok(await _routeServices.GetRoute(routeId));

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
