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
using System.Linq;

namespace Inventory.Domain.Implementation.Product
{
    public class ProductVariantService : ServiceBase, IProductVariantService
    {
        public ProductVariantService(
            ILogger<ProductVariantService> logger,
            IObjectMapper mapper,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IUnitOfWorkManager unitOfWork,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor) { }

        public async Task<IEnumerable<ProductVariantModel>> SaveNewVariantsAsync(IRepositoryFactory factory, IEnumerable<ProductVariantModel> productVariants)
        {
            IRepositoryBase<ProductVariant> productVariantRepo = factory.GetRepository<ProductVariant>();

            IEnumerable<ProductVariant> entityList = Mapper.Map<IEnumerable<ProductVariant>>(productVariants);

            IEnumerable<ProductVariant> newItems = entityList.Where(x => x.VariantId <= 0).ToList();
            IEnumerable<ProductVariant> existingItems = entityList.Where(x => x.VariantId > 0).ToList();

            foreach(ProductVariant variantEntity in newItems){
                variantEntity.CreateBy = User.UserId;
                variantEntity.CreateDate = DateTime.UtcNow;
                variantEntity.IsActive = true;
            }
            newItems = await productVariantRepo.BulkInsertAsync(newItems);

            foreach (ProductVariant item in existingItems) {
                productVariantRepo.ColumnUpdate(item.VariantId, new Dictionary<string, object>
                {
                    { "VariantName", item.VariantName },
                    { "SKU", item.SKU },
                    { "BarCode", item.BarCode },
                    { "Attributes", item.Attributes },
                    { "UpdateBy", User.UserId },
                    { "UpdateDate", DateTime.UtcNow },
                    { "ProductId", item.ProductId },
                    { "IsActive", true },
                });
            }
            await productVariantRepo.SaveChangesAsync();

            List<int> newAndExistingRecordIds = existingItems.Select(existingVariants => existingVariants.VariantId).ToList();
            newAndExistingRecordIds.AddRange(newItems.Select(newVariants => newVariants.VariantId).ToList());

            int[] deletedItemIds = await productVariantRepo.UnTrackableQuery()
                .Where(x => x.ProductId == entityList.First().ProductId 
                    && !newAndExistingRecordIds.Contains(x.VariantId)
                    && x.IsActive)
                .Select(x => x.VariantId)
                .ToArrayAsync();

            foreach (int itemId in deletedItemIds)
            {
                productVariantRepo.ColumnUpdate(itemId, new Dictionary<string, object>
                    {
                        { "UpdateBy", User.UserId },
                        { "UpdateDate", DateTime.UtcNow },
                        { "IsActive", false },
                    });
            }

            await productVariantRepo.SaveChangesAsync();

            return productVariants;
        }

        public async Task<IEnumerable<ProductVariantModel>> SaveNewVariantsAsync(int ProductId, IEnumerable<ProductVariantModel> productVariants)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                return await SaveNewVariantsAsync(_repositoryFactory, productVariants);
            }
        }

        public async Task DeleteAsync(int productVariantId)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var productVariantRepo = _repositoryFactory.GetRepository<ProductVariant>();
                productVariantRepo.ColumnUpdate(productVariantId, new Dictionary<string, object> {
                    { "IsActive", false }
                });
                await productVariantRepo.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProductVariantModel>> GetVariantListOfProduct(int productId)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var productVariantRepo = _repositoryFactory.GetRepository<ProductVariant>();
                return await productVariantRepo.UnTrackableQuery()
                    .Where(variant  => variant.ProductId == productId && variant.IsActive)
                    .Select(variant => new ProductVariantModel
                    {
                        VariantId = variant.ProductId,
                        VariantName = variant.VariantName,
                        SKU = variant.SKU,
                        StockQuantity = variant.StockQuantity,
                        BarCode = variant.BarCode,
                        Attributes = variant.Attributes,
                        ProductId = variant.ProductId,
                    })
                    .ToListAsync();
            }
        }

        public async Task UpdateAsync(int VariantId, Dictionary<string, object> productVariant)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var productVariantRepo = _repositoryFactory.GetRepository<ProductVariant>();
                productVariantRepo.ColumnUpdate(VariantId, productVariant);
                await productVariantRepo.SaveChangesAsync();
            }
        }
    }
}
