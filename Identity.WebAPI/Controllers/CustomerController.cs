using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebAPI.Controllers
{
    /// <summary>
    /// Customer Module
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(ICustomerService customerService) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;

        /// <summary>
        /// Get list of organizations that the user is tagged with
        /// </summary>
        [HttpPost]
        [Route("Onboarding")]
        public async Task<IActionResult> OnboardUser(OnboardingModel onboardingModel)
        {
            await _customerService.OnboardUser(onboardingModel.User, onboardingModel.Organization);
            return Ok();
        }
    }
}
