using RedBook.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Models
{
    public class PagedPurchaseInvoiceModel: PagedModel<PurchaseInvoiceModel>
    {
        public string ChalanNumber { get; set; }
        public string CheckNumber { get; set; }
        public int OrganizationId { get; set; }
    }
}
