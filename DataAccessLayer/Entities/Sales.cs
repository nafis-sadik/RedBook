using RedBook.Core.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    // Sales Id shall be considered as Memo Number
    public class Sales : BaseEntity<int>
    {
        public DateTime SalesDate { get; set; }
        public Guid SoldBy { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual ICollection<SalesDetails> SalesDetails { get; set; } = new List<SalesDetails>(); 
        public virtual ICollection<SalesPaymentRecords> SalesPaymentRecords { get; set; } = new List<SalesPaymentRecords>();
    }
}
