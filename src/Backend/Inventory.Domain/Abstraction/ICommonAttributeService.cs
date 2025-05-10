using Inventory.Data.Models;

namespace Inventory.Domain.Abstraction
{
    public interface ICommonAttributeService
    {
        public Task<CommonAttributeModel> AddCommonAttributeAsync(CommonAttributeModel CommonAttributeModel);
        public Task<IEnumerable<CommonAttributeModel>> GetByTypeAsync(string attributeType);
        public Task DeleteCommonAttributeAsync(int attributeId);
        public Task<CommonAttributeModel> UpdateCommonAttributeAsync(CommonAttributeModel CommonAttributeModel);
    }
}
