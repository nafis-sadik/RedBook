using Inventory.Data.Models.CRM;
using Inventory.Data.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Abstraction.CRM
{
    public interface ICustomerServices
    {
        public Task<CustomerModel?> SyncCustomerInfoAsync(CustomerModel model);
        public Task<string[]> SearchByContactNumberFromPurchaseHistory(string searchString, int orgId);
        public Task<CustomerModel?> FindCustomerByContactNumber(string searchString, int orgId);
    }
}
