using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(int orgId) => Ok(await _organizationService.GetOrganizationAsync(orgId));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(OrganizationModel orgInfo) => Ok(await _organizationService.AddOrganizationAsync(orgInfo));

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(OrganizationModel orgInfo) => Ok(await _organizationService.UpdateOrganizationAsync(orgInfo));

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int orgId)
        {
            await _organizationService.DeleteOrganizationAsync(orgId);
            return Ok();
        }
    }
}
