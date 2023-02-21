using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory.Data.Entities
{
    public partial class Sale
    {
        public Sale()
        {
            SalesDetails = new HashSet<SalesDetail>();
            SalesPaymentRecords = new HashSet<SalesPaymentRecord>();
        }

        public string Id { get; set; }
        public DateTime SalesDate { get; set; }
        public string SoldBy { get; set; }
        public decimal TotalAmount { get; set; }
        public int OrganizationId { get; set; }

        public virtual ICollection<SalesDetail> SalesDetails { get; set; }
        public virtual ICollection<SalesPaymentRecord> SalesPaymentRecords { get; set; }
    }
}
