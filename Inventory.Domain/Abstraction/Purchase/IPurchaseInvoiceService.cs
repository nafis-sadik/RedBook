using Inventory.Data.Models.Purchase;
using RedBook.Core.Models;

namespace Inventory.Domain.Abstraction.Purchase
{
    public interface IPurchaseService
    {
        public Task AddNewAsync(RecordModel purchaseModel);
        public Task<PagedModel<RecordModel>> GetPagedAsync(PagedModel<RecordModel> purchaseModel);
        public Task<RecordModel> UpdateAsync(RecordModel purchaseModel);
        public Task DeleteAsync(int id);
    }
}
