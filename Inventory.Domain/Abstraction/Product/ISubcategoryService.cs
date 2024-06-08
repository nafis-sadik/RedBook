using Inventory.Data.Models.Product;

namespace Inventory.Domain.Abstraction.Product
{
    public interface ISubcategoryService
    {
        public Task<CategoryModel> AddSubcategoryAsync(CategoryModel categoryModel);
        public Task<IEnumerable<CategoryModel>> GetSubcategoriesUnderCategory(int categoryId);
        public Task DeleteSubcategoryAsync(int categoryId);
        public Task<CategoryModel> UpdateSubcategoryAsync(CategoryModel categoryModel);
    }
}
