using Inventory.Data.Entities;
using Inventory.Data.Models.Product;
using Inventory.Domain.Abstraction.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Domain;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Inventory.Domain.Implementation.Product
{
    public class CategoryService : ServiceBase, ICategoryService
    {
        private IRepositoryFactory _repositoryFactory;
        private IRepositoryBase<Category> _categoryRepo;
        public CategoryService(
            ILogger<CategoryService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor) { }

        public async Task<CategoryModel> AddCategoryAsync(CategoryModel categoryModel)
        {
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _categoryRepo = _repositoryFactory.GetRepository<Category>();

                Category category = await _categoryRepo.InsertAsync(new Category
                {
                    CatagoryName = categoryModel.CatagoryName,
                    CreatedBy = User.UserId,
                    CreateDate = DateTime.UtcNow,
                    OrganizationId = categoryModel.OrganizationId
                });

                await _repositoryFactory.SaveChangesAsync();

                CategoryModel data = Mapper.Map<CategoryModel>(category);

                return data;
            }
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _categoryRepo = _repositoryFactory.GetRepository<Category>();
                Category? category = await _categoryRepo.GetAsync(categoryId);
                if (category == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);

                int id = 0;
                if (category.ParentCategoryId == null)
                    id = await _categoryRepo.TrackableQuery().Where(x => x.ParentCategoryId == categoryId).ExecuteDeleteAsync();

                await _categoryRepo.DeleteAsync(categoryId);

                await _repositoryFactory.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CategoryModel>> GetByOrganizationAsync(int orgId)
        {
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _categoryRepo = _repositoryFactory.GetRepository<Category>();

                return await _categoryRepo.UnTrackableQuery()
                    .Where(x => x.OrganizationId == orgId && x.ParentCategoryId == null)
                    .Select(x => new CategoryModel
                    {
                        CategoryId = x.CategoryId,
                        CatagoryName = x.CatagoryName,
                        OrganizationId = x.OrganizationId,
                    }).ToListAsync();
            }
        }

        public async Task<CategoryModel> UpdateCategoryAsync(CategoryModel categoryModel)
        {
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _categoryRepo = _repositoryFactory.GetRepository<Category>();

                Category? category = await _categoryRepo.GetAsync(categoryModel.CategoryId);

                if (category == null) throw new ArgumentException($"Resource not found");

                category.CatagoryName = categoryModel.CatagoryName;
                category.UpdateDate = DateTime.UtcNow;
                category.UpdatedBy = User.UserId;

                _categoryRepo.Update(category);
                await _repositoryFactory.SaveChangesAsync();

                return Mapper.Map<CategoryModel>(categoryModel);
            }
        }
    }
}
