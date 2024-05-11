using Identity.Data.Models;

namespace Identity.Domain.Abstraction
{
    public interface IOnboardingService
    {
        public Task RedbookOnboardingAsync(OnboardingModel model);
    }
}
