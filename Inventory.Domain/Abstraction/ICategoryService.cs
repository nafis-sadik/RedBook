using Inventory.Data.Models;

namespace Inventory.Domain.Abstraction
{
    public interface ICategoryService
    {
        public Task<CategoryModel> AddCategoryAsync(CategoryModel categoryModel);
        public Task<IEnumerable<CategoryModel>> GetByOrganizationAsync(int orgId);
        public Task DeleteCategoryAsync(int categoryId);
        public Task<CategoryModel> UpdateCategoryAsync(CategoryModel categoryModel);
    }
}
