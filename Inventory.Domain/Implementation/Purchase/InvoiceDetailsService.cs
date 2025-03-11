using Inventory.Data.Entities;
using Inventory.Data.Models.Purchase;
using Inventory.Domain.Abstraction.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Domain;
using RedBook.Core.Models;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Inventory.Domain.Implementation.Purchase
{
    public class InvoiceDetailsService : ServiceBase, IInvoiceDetailsService
    {
        public InvoiceDetailsService(
            ILogger<InvoiceDetailsService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor) { }
        public async Task<List<InvoiceDetailsModel>> AddNewAsync(List<InvoiceDetailsModel> purchaseModel)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _invoiceDetailsRepo = factory.GetRepository<PurchaseInvoiceDetails>();

                IEnumerable<PurchaseInvoiceDetails> entities = Mapper.Map<List<PurchaseInvoiceDetails>>(purchaseModel);

                await _invoiceDetailsRepo.BulkInsertAsync(entities);

                await _invoiceDetailsRepo.SaveChangesAsync();

                return purchaseModel;
            }
        }

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task<PagedModel<InvoiceDetailsModel>> GetPagedAsync(PagedModel<InvoiceDetailsModel> purchaseModel) => throw new NotImplementedException();

        public async Task<IEnumerable<InvoiceDetailsModel>> GetListAsync(int invoiceId)
        {
            using(var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _invoiceDetailsRepo = factory.GetRepository<PurchaseInvoiceDetails>();

                return await _invoiceDetailsRepo.UnTrackableQuery()
                    .Where(invoiceDetails => invoiceDetails.InvoiceId == invoiceId)
                    .Select(invoiceDetails => new InvoiceDetailsModel
                    {
                        VariantId = invoiceDetails.ProductVariantId == null? 0 : invoiceDetails.ProductVariantId,
                        ProductName = invoiceDetails.ProductName,
                        ProductVariantName = invoiceDetails.ProductVariantName,
                        Quantity = invoiceDetails.Quantity,
                        PurchasePrice = invoiceDetails.PurchasePrice,
                        PurchaseDiscount = invoiceDetails.PurchaseDiscount,
                        RetailPrice = invoiceDetails.RetailPrice,
                        MaxRetailDiscount = invoiceDetails.MaxRetailDiscount,
                        VatRate = invoiceDetails.VatRate,
                        BarCode = invoiceDetails.BarCode
                    }).ToListAsync();
            }
        }

        public Task<InvoiceDetailsModel> UpdateAsync(InvoiceDetailsModel purchaseModel) => throw new NotImplementedException();
    }
}