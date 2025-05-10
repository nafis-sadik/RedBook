namespace Inventory.Data.Models.Purchase
{
    public class VendorModel
    {
        public int VendorId { get; set; }

        public string VendorName { get; set; }

        public string ContactPerson { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string EmailAddress { get; set; }

        public string Remarks { get; set; }

        public int OrganizationId { get; set; }

        public float TotalPayable { get; set; }

        public float TotalRecievable { get; set; }

        public int ApplicationId { get; set; }
    }
}
