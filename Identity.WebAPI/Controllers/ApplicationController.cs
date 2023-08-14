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
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int appId) => Ok(await _applicationService.GetApplicationAsync(appId));

        [HttpPost]
        public async Task<IActionResult> AddAsync(ApplicationInfoModel appInfo) => Ok(await _applicationService.AddApplicationAsync(appInfo));

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(ApplicationInfoModel appInfo) => Ok(await _applicationService.UpdateApplicationAsync(appInfo));

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int appId)
        {
            await _applicationService.DeleteApplicationAsync(appId);
            return Ok();
        }

        [HttpGet]
        [Route("/PagedApplications")]
        public async Task<IActionResult> GetPagedAsync([FromQuery] PagedModel<ApplicationInfoModel> pagedApplications) => Ok(await _applicationService.GetApplicationsAsync(pagedApplications));
    }
}