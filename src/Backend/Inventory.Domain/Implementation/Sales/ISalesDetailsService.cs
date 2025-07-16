using Inventory.Data.Entities;
using Inventory.Data.Models.Sales;
using Inventory.Domain.Abstraction.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Domain;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Inventory.Domain.Implementation.Sales
{
    public class SalesDetailsService : ServiceBase, ISalesDetailsService
    {
        public SalesDetailsService(
            ILogger<SalesDetailsService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor) { }

        public async Task<List<SalesInvoiceDetailsModel>> GetByInvoiceId(int invoiceId)
        {
            using(var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var salesRepo = factory.GetRepository<SalesInvoice>();
                var salesDetailsRepo = factory.GetRepository<SalesInvoiceDetails>();

                return await salesDetailsRepo.UnTrackableQuery()
                    .Where(saledDetails => saledDetails.InvoiceId == invoiceId)
                    .Select(saledDetails => new SalesInvoiceDetailsModel
                    {
                        InvoiceId = saledDetails.InvoiceId,
                        ProductName = saledDetails.ProductName,
                        ProductVariantName = saledDetails.ProductVariantName,
                        Lot = new Data.Models.Purchase.PurchaseInvoiceDetailsModel
                        {
                            RecordId = saledDetails.PurchaseInvoiceDetails.RecordId,
                            PurchasePrice = saledDetails.PurchaseInvoiceDetails.PurchasePrice,
                            PurchaseDiscount = saledDetails.PurchaseInvoiceDetails.PurchaseDiscount
                        },
                        Quantity = saledDetails.Quantity,
                        RetailPrice = saledDetails.RetailPrice,
                        RetailDiscount = saledDetails.RetailDiscount,
                        VatRate = saledDetails.VatRate,
                        TotalCostPrice = (saledDetails.RetailPrice * saledDetails.Quantity) - saledDetails.RetailDiscount,
                    }).ToListAsync();
            }
        }
    }
}
