﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data.Entities
{
    [Table("Vendors")]
    public class Vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string Remarks { get; set; }
        public int OrganizationId { get; set; }
        public int ApplicationId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public virtual ICollection<PurchaseInvoice> PurchaseInvoices { get; set; } = new List<PurchaseInvoice>();
    }
}
