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

namespace Inventory.Domain.Implementation
{
    public class CategoryService : ServiceBase, ICategoryService
    {
        private IRepositoryBase<Category> _categoryRepo;
        public CategoryService(
            ILogger<CategoryService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork) { }

        public async Task<CategoryModel> AddCategoryAsync(CategoryModel categoryModel)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _categoryRepo = unitOfWork.GetRepository<Category>();

                Category category = await _categoryRepo.InsertAsync(new Category
                {
                    CatagoryName = categoryModel.CatagoryName,
                    CreateDate = DateTime.Now,
                    CreatedBy = User.UserId,
                    OrganizationId = categoryModel.BusinessId
                });

                await unitOfWork.SaveChangesAsync();

                CategoryModel data = Mapper.Map<CategoryModel>(category);

                return data;
            }
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {

            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _categoryRepo = unitOfWork.GetRepository<Category>();
                await _categoryRepo.DeleteAsync(categoryId);
            }
        }

        public async Task<IEnumerable<CategoryModel>> GetByOrganizationAsync(int orgId)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _categoryRepo = unitOfWork.GetRepository<Category>();

                List<CategoryModel> data = await _categoryRepo.UnTrackableQuery()
                    .Where(x => x.OrganizationId == orgId)
                    .Select(x => new CategoryModel
                    {
                        CategoryId = x.CategoryId,
                        CatagoryName = x.CatagoryName,
                        BusinessId = x.OrganizationId,
                    }).ToListAsync();

                return data;
            }
        }

        public async Task<CategoryModel> UpdateCategoryAsync(CategoryModel categoryModel)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _categoryRepo = unitOfWork.GetRepository<Category>();

                Category? category = await _categoryRepo.GetAsync(categoryModel.CategoryId);

                if (category == null) throw new ArgumentException($"Resource not found");

                category.CatagoryName = categoryModel.CatagoryName;
                category.UpdateDate = DateTime.Now;
                category.CreatedBy = User.UserId;

                _categoryRepo.Update(category);
                await unitOfWork.SaveChangesAsync();

                return Mapper.Map<CategoryModel>(categoryModel);
            }
        }
    }
}
