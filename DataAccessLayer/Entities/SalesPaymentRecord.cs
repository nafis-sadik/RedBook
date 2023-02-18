using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory.Domain.Entities
{
    public partial class SalesPaymentRecord
    {
        public int Id { get; set; }
        public string MemoNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }

        public virtual Sale MemoNumberNavigation { get; set; }
    }
}
