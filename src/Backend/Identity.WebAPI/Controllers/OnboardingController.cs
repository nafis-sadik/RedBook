using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnboardingController(IOnboardingService onboardingService) : ControllerBase
    {
        private readonly IOnboardingService _onboardingService = onboardingService;

        [HttpPost]
        [Route("Redbook")]
        public async Task<IActionResult> RedbookOnboarding(OnboardingModel onboardingModel)
        {
            await _onboardingService.RedbookOnboardingAsync(onboardingModel);
            return Ok();
        }
    }
}
