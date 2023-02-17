using Org.BouncyCastle.Asn1.Crmf;
using RedBook.Core.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class SalesDetails : BaseEntity<int>
    {
        public int MemoNumber { get; set; }
        public int ProductId { get; set; }
        public int ChalanNo { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Sales Sales { get; set; } = new Sales();
    }
}
