using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;
        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(int policyId) => Ok(await _policyService.GetPolicyAsync(policyId));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(PolicyModel policyDetails) => Ok(await _policyService.AddPolicyAsync(policyDetails));

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(PolicyModel policyDetails) => Ok(await _policyService.UpdatePolicyAsync(policyDetails));

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int policyId)
        {
            await _policyService.DeletePolicyAsync(policyId);
            return Ok();
        }
    }
}
