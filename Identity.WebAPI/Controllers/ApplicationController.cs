using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(int appId) => Ok(await _applicationService.GetApplication(appId));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(ApplicationInfoModel appInfo) => Ok(await _applicationService.AddApplication(appInfo));

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(ApplicationInfoModel appInfo) => Ok(await _applicationService.UpdateApplication(appInfo));

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int appId)
        {
            await _applicationService.DeleteApplication(appId);
            return Ok();
        }
    }
}
