namespace Identity.Data.Models
{
    public class SubscriptionModel
    {
        public int SubscriptionId { get; set; }
        public DateTime RenewalDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
