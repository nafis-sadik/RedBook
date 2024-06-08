using Inventory.Data.Entities;
using Inventory.Data.Models;
using Inventory.Data.Models.Purchase;
using Inventory.Domain.Abstraction.Purchase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Domain;
using RedBook.Core.Models;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Inventory.Domain.Implementation.Purchase
{
    public class ProductPurchaseInvoice : ServiceBase, IPurchaseInvoiceService
    {
        public ProductPurchaseInvoice(
            ILogger<ProductPurchaseInvoice> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork) { }

        public async Task<InvoiceModel> GetByIdAsync(int id)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _invoiceRepo = factory.GetRepository<PurchaseInvoice>();

                PurchaseInvoice? entity = await _invoiceRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.InvoiceId == id);
                if (entity == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);

                return Mapper.Map<InvoiceModel>(entity);
            }
        }

        public async Task<PagedInvoiceModel> GetPagedAsync(PagedInvoiceModel pagedModel)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _purchaseInvoiceRepo = factory.GetRepository<PurchaseInvoice>();

                var query = _purchaseInvoiceRepo.UnTrackableQuery().Where(i => i.OrganizationId == pagedModel.OrganizationId);

                if (!string.IsNullOrEmpty(pagedModel.SearchString))
                    query = query.Where(i => i.ChalanNumber.ToLower().Contains(pagedModel.SearchString));

                pagedModel.TotalItems = await query.CountAsync();

                query = query.Skip(pagedModel.Skip)
                    .Take(pagedModel.PageLength)
                    .OrderByDescending(i => i.CreateDate);

                pagedModel.SourceData = await query.Select(i => new InvoiceModel
                {
                    InvoiceId = i.InvoiceId,
                    ChalanNumber = i.ChalanNumber,
                    TotalPurchasePrice = i.TotalPurchasePrice,
                }).ToListAsync();

                return pagedModel;
            }
        }

        public async Task<InvoiceModel> AddNewAsync(InvoiceModel model)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _purchaseInvoiceRepo = factory.GetRepository<PurchaseInvoice>();

                var entity = Mapper.Map<PurchaseInvoice>(model);

                entity = await _purchaseInvoiceRepo.InsertAsync( entity );

                return Mapper.Map<InvoiceModel>(entity);
            }
        }

        public async Task<InvoiceModel> UpdateAsync(int id, Dictionary<string, object> updates)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _purchaseInvoiceRepo = factory.GetRepository<PurchaseInvoice>();

                _purchaseInvoiceRepo.ColumnUpdate(id, updates);

                await factory.SaveChangesAsync();

                var entity = _purchaseInvoiceRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.InvoiceId == id);

                return Mapper.Map<InvoiceModel>(entity);
            }
        }
        
        public async Task DeleteAsync(int id)
        {
            await UpdateAsync(id, new Dictionary<string, object>
            {
                { "IsDeleted", false }
            });
        }
    }
}
