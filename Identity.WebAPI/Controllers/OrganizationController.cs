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
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int orgId) => Ok(await _organizationService.GetOrganizationAsync(orgId));

        [HttpPost]
        public async Task<IActionResult> Add(OrganizationModel orgInfo) => Ok(await _organizationService.AddOrganizationAsync(orgInfo));

        [HttpPut]
        public async Task<IActionResult> Update(OrganizationModel orgInfo) => Ok(await _organizationService.UpdateOrganizationAsync(orgInfo));

        [HttpGet]
        [Route("/PagedOrganizations")]
        public async Task<IActionResult> GetPagedAsync([FromQuery] PagedModel<OrganizationModel> orgPagedCollection) => Ok(await _organizationService.GetOrganizationsAsync(orgPagedCollection));

        [HttpDelete]
        public async Task<IActionResult> Delete(int orgId)
        {
            await _organizationService.DeleteOrganizationAsync(orgId);
            return Ok();
        }
    }
}
