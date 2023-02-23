//using Microsoft.EntityFrameworkCore;
//using Identity.Domain.Abstraction;
//using RedBook.Core.Domain;

//namespace Identity.Domain.Implementation
//{
//    public class SubscriptionService : ServiceBase, ISubscriptionService
//    {
//        private readonly ISubscriptionLogRepo _subscriptionLogRepo;
//        private readonly IServiceRepo _subscriptionRepo;
//        private readonly ICrashLogRepo _crashLogRepo;

//        public SubscriptionService(ISubscriptionLogRepo subscriptionLogRepo, IServiceRepo subscriptionRepo)
//        {
//            OrcusSMEContext context = new OrcusSMEContext(new DbContextOptions<OrcusSMEContext>());
//            _subscriptionLogRepo = subscriptionLogRepo;
//            _subscriptionRepo = subscriptionRepo;
//            _crashLogRepo = new CrashLogRepo(context);
//        }

//        public IEnumerable<SubscribedService> GetActiveSubscriptions(string userId)
//        {
//            IQueryable<SubscriptionLog> subscriptionLogs = _subscriptionLogRepo.AsQueryable().Where(x => x.SubscriptionDate > DateTime.Now);
//            List<SubscribedService> subscriptions = new List<SubscribedService>();
//            foreach(SubscriptionLog subscriptionHistory in subscriptionLogs)
//            {
//                subscriptions.Add(new SubscribedService {
//                    SubscriptionId = (int)subscriptionHistory.SubscriptionId,
//                    //ServiceName = subscriptionHistory.Subscription.SubscriptionName,
//                    SubscriptionName = "Dana Shop"
//                });
//            }
//            return subscriptions;
//        }

//        public IEnumerable<SubscribedService> GetSubscriptionHistory(BaseModel pagination, string userId)
//        {
//            IQueryable<SubscriptionLog> SubscriptionLogs = _subscriptionLogRepo.AsQueryable().Where(x => x.UserId == userId).Skip(pagination.Skip).Take(pagination.PageSize);
//            List<SubscribedService> subscriptions = new List<SubscribedService>();
//            foreach (SubscriptionLog subscriptionHistory in SubscriptionLogs)
//            {
//                subscriptions.Add(new SubscribedService
//                {
//                    SubscriptionId = (int)subscriptionHistory.SubscriptionId,
//                    //ServiceName = subscriptionHistory.Subscription.SubscriptionName,
//                    SubscriptionName = "Dana Shop",
//                    ExpirationDate = subscriptionHistory.ExpirationDate,
//                    SubscriptionDate = subscriptionHistory.SubscriptionDate,
//                    //SubscriptionPrice = (int)subscriptionHistory.Subscription.SubscriptionPrice
//                });
//            }
//            return subscriptions;
//        }

//        public bool Subscribe(string UserId, int SubscriptionId)
//        {
//            try
//            {
//                IQueryable<SubscriptionLog> subscriptionLogs = _subscriptionLogRepo.AsQueryable().Where(x => x.UserId == UserId && x.SubscriptionId == SubscriptionId && x.ExpirationDate > DateTime.Today);
//                Service subscriptions = _subscriptionRepo.Get(SubscriptionId);
//                if (subscriptionLogs == null)
//                    _subscriptionLogRepo.Add(new SubscriptionLog
//                    {
//                        SubscriptionId = SubscriptionId,
//                        SubscriptionDate = DateTime.Now,
//                        ExpirationDate = DateTime.Now.AddMonths((int)_subscriptionRepo.Get(SubscriptionId).DurationMonths),
//                        UserId = UserId
//                    });
//                else
//                {
//                    foreach (SubscriptionLog subscription in subscriptionLogs)
//                    {
//                        subscription.ExpirationDate = DateTime.Now.AddMonths((int)_subscriptionRepo.Get(SubscriptionId).DurationMonths);
//                        _subscriptionLogRepo.Update(subscription);
//                    }
//                }
//                return true;
//            } catch (Exception ex) {
//                _crashLogRepo.Add(new Crashlog
//                {
//                    ClassName = "SubscriptionService",
//                    Data = UserId.ToString() + " " + SubscriptionId.ToString(),
//                    ErrorInner = ex.InnerException != null ? ex.InnerException.Message : "",
//                    ErrorMessage = ex.Message,
//                    MethodName = "Subscribe",
//                    TimeStamp = DateTime.Now
//                });
//                return false;
//            }
//        }

//        public bool HasSubscription(string userId, int subscriptionId) => _subscriptionLogRepo.AsQueryable().Where(x => x.UserId == userId && x.SubscriptionId == subscriptionId) != null;
//    }
//}
