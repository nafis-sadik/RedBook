using Inventory.Data.Models.Purchase;
using RedBook.Core.Models;

namespace Inventory.Domain.Abstraction.Purchase
{
    public interface IVendorService
    {
        public Task<VendorModel> GetByIdAsync(int id);
        public Task<IEnumerable<VendorModel>> GetByOrgId(int orgId);
        public Task<PagedModel<VendorModel>> GetPagedAsync(PagedModel<VendorModel> pagedModel);
        public Task<VendorModel> AddNewAsync(VendorModel model);
        public Task<VendorModel> UpdateAsync(int id, Dictionary<string, object> updates);
        public Task DeleteAsync(int id);
    }
}
