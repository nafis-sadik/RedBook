using Inventory.Data.Models;

namespace Inventory.Domain.Abstraction
{
    public interface ISubcategoryService
    {
        public Task<CategoryModel> AddSubcategoryAsync(CategoryModel categoryModel);
        public Task<IEnumerable<CategoryModel>> GetSubcategoriesUnderCategory(int categoryId);
        public Task DeleteSubcategoryAsync(int categoryId);
        public Task<CategoryModel> UpdateSubcategoryAsync(CategoryModel categoryModel);
    }
}
