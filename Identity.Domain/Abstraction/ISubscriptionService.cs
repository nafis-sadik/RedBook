using Identity.Data.Models;
using RedBook.Core.Models;

namespace Identity.Domain.Abstraction
{
    public interface ISubscriptionService
    {
        public bool Subscribe(string UserId, int SubscriptionId);
        //public IEnumerable<SubscribedService> GetActiveSubscriptions(string userId);
        //public PagedModel<SubscribedService> GetSubscriptionHistory(PagedModel<UserModel> pagination, string userId);
        public bool HasSubscription(string userId, int subscriptionId);
    }
}
