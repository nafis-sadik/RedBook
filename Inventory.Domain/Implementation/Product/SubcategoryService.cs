using Inventory.Data.Entities;
using Inventory.Data.Models.Product;
using Inventory.Domain.Abstraction.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Domain;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Inventory.Domain.Implementation.Product
{
    public class SubcategoryService : ServiceBase, ISubcategoryService
    {
        private IRepositoryFactory _repositoryFactory;
        private IRepositoryBase<Category> _categoryRepo;
        public SubcategoryService(
            ILogger<SubcategoryService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor) { }

        public async Task<CategoryModel> AddSubcategoryAsync(CategoryModel categoryModel)
        {
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _categoryRepo = _repositoryFactory.GetRepository<Category>();
                Category category = Mapper.Map<Category>(categoryModel);
                category.CreateDate = DateTime.UtcNow;
                category.CreatedBy = User.UserId;

                category = await _categoryRepo.InsertAsync(category);

                await _repositoryFactory.SaveChangesAsync();

                CategoryModel data = Mapper.Map<CategoryModel>(category);

                return data;
            }
        }

        public async Task DeleteSubcategoryAsync(int categoryId)
        {
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _categoryRepo = _repositoryFactory.GetRepository<Category>();

                // Remove corresponding children first
                await _categoryRepo.TrackableQuery().Where(x => x.ParentCategoryId == categoryId).ExecuteDeleteAsync();

                // Then remove the target item
                await _categoryRepo.DeleteAsync(categoryId);

                await _repositoryFactory.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CategoryModel>> GetSubcategoriesUnderCategory(int categoryId)
        {
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _categoryRepo = _repositoryFactory.GetRepository<Category>();

                var data = await _categoryRepo
                    .UnTrackableQuery()
                    .Where(x => x.ParentCategoryId == categoryId)
                    .Select(x => new CategoryModel
                    {
                        CategoryId = x.CategoryId,
                        CatagoryName = x.CatagoryName,
                        ParentCategoryId = x.ParentCategoryId,
                    })
                    .ToListAsync();

                return data;
            }
        }

        public async Task<CategoryModel> UpdateSubcategoryAsync(CategoryModel categoryModel)
        {
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _categoryRepo = _repositoryFactory.GetRepository<Category>();

                Category? category = await _categoryRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.CategoryId == categoryModel.CategoryId);
                if (category == null) throw new ArgumentException("Resource not found");

                category.CatagoryName = categoryModel.CatagoryName;
                category.ParentCategoryId = categoryModel.ParentCategoryId;
                category.UpdateDate = DateTime.UtcNow;
                category.UpdatedBy = User.UserId;

                _categoryRepo.Update(category);

                await _repositoryFactory.SaveChangesAsync();

                return Mapper.Map<CategoryModel>(categoryModel);
            }
        }
    }
}
