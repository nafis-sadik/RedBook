using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Models
{
    public class InventoryModel
    {
        public int InventoryId { get; set; }

        public int ProductId { get; set; }

        public decimal Quantity { get; set; }

        public int PurchaseId { get; set; }

        public int QuantityAttributeId { get; set; }

        public int OrganizationId { get; set; }
    }
}
