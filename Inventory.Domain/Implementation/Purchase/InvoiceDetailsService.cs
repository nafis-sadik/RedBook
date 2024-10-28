using Inventory.Data.Entities;
using Inventory.Data.Models.Purchase;
using Inventory.Domain.Abstraction.Purchase;
using Microsoft.AspNetCore.Http;
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
        public Task<InvoiceDetailsModel> UpdateAsync(InvoiceDetailsModel purchaseModel) => throw new NotImplementedException();
    }
}
