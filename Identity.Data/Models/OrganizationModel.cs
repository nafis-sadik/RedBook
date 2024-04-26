namespace Identity.Data.Models
{
    public class OrganizationModel
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        public string BusinessLogo { get; set; }
        public float SubscriptionFee { get; set; }
    }
}
