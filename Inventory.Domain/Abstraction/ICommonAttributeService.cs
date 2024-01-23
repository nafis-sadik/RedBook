using Inventory.Data.Models;

namespace Inventory.Domain.Abstraction
{
    public interface ICommonAttributeService
    {
        public Task<CommonAttributeModel> AddCategoryAsync(CommonAttributeModel CommonAttributeModel);
        public Task<IEnumerable<CommonAttributeModel>> GetByTypeAsync(string attributeType);
        public Task DeleteCategoryAsync(int attributeId);
        public Task<CommonAttributeModel> UpdateCategoryAsync(CommonAttributeModel CommonAttributeModel);
    }
}
