using Inventory.Data.Entities;
using Inventory.Data.Models;
using Inventory.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Domain;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Implementation
{
    public class SubcategoryService : ServiceBase, ISubcategoryService
    {
        private IRepositoryBase<Category> _categoryRepo;
        public SubcategoryService(
            ILogger<CategoryService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork) { }

        public async Task<CategoryModel> AddSubcategoryAsync(CategoryModel categoryModel)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _categoryRepo = unitOfWork.GetRepository<Category>();

                Category category = await _categoryRepo.InsertAsync(new Category
                {
                    CatagoryName = categoryModel.CatagoryName,
                    CreateDate = DateTime.Now,
                    CreatedBy = User.UserId,
                    OrganizationId = categoryModel.BusinessId,
                    ParentCategory = categoryModel.ParentCategory,
                });

                await unitOfWork.SaveChangesAsync();

                CategoryModel data = Mapper.Map<CategoryModel>(category);

                return data;
            }
        }

        public async Task DeleteSubcategoryAsync(int categoryId)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _categoryRepo = unitOfWork.GetRepository<Category>();
                await _categoryRepo.DeleteAsync(x => x.ParentCategory == categoryId);
                await unitOfWork.SaveChangesAsync();
                await _categoryRepo.DeleteAsync(categoryId);
                await unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CategoryModel>> GetSubcategoriesUnderCategory(int categoryId)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _categoryRepo = unitOfWork.GetRepository<Category>();

                var data = await _categoryRepo.UnTrackableQuery()
                    .Where(x => x.ParentCategory == categoryId)
                    .Select(x => new CategoryModel
                    {
                        CategoryId = x.CategoryId,
                        CatagoryName = x.CatagoryName,
                        ParentCategory = x.ParentCategory,
                    })
                    .ToListAsync();

                return data;
            }
        }

        public async Task<CategoryModel> UpdateSubcategoryAsync(CategoryModel categoryModel)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _categoryRepo = unitOfWork.GetRepository<Category>();

                Category? category = await _categoryRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.CategoryId == categoryModel.CategoryId);
                if (category == null) throw new ArgumentException("Resource not found");

                category.CatagoryName = categoryModel.CatagoryName;
                category.ParentCategory = categoryModel.ParentCategory;
                category.UpdateDate = DateTime.UtcNow;
                category.UpdatedBy = User.UserId;

                _categoryRepo.Update(category);

                await unitOfWork.SaveChangesAsync();

                return Mapper.Map<CategoryModel>(categoryModel);
            }
        }
    }
}
