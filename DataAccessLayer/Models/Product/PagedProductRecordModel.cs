using RedBook.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Models.Product
{
    public class PagedProductRecordModel : PagedModel<ProductModel>
    {
        public int OrganizationId { get; set; }
    }
}
