namespace Inventory.Data.Models.Purchase
{
    public class VendorModel
    {
        public int VendorId { get; set; }

        public string VendorName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Remarks { get; set; }

        public int OrganizationId { get; set; }

        public int ApplicationId { get; set; }
    }
}
