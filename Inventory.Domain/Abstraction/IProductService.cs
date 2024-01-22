using Inventory.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Abstraction
{
    public interface IProductService
    {
        public Task<CategoryModel> AddSubcategoryAsync(ProductModel categoryModel);
        public Task<IEnumerable<CategoryModel>> GetSubcategoriesUnderCategory(int categoryId);
        public Task DeleteSubcategoryAsync(int categoryId);
        public Task<CategoryModel> UpdateSubcategoryAsync(CategoryModel categoryModel);
    }
}
