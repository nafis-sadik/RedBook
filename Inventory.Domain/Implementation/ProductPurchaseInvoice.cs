using Inventory.Clients;
using Inventory.Data.Entities;
using Inventory.Data.Models;
using Inventory.Domain.Abstraction;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Domain;
using RedBook.Core.Models;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Inventory.Domain.Implementation
{
    public class ProductPurchaseInvoice : ServiceBase, IProductPurchaseInvoice
    {
        private IRepositoryBase<Purchase> _purchaseRepo;
        public ProductPurchaseInvoice(
            ILogger<ProductPurchaseInvoice> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork) { }

        public Task AddNewInvoiceAsync(PurchaseModel purchaseModel) => throw new NotImplementedException();

        public async Task<PagedModel<PurchaseModel>> GetPagedInvoiceAsync(PagedModel<PurchaseModel> purchaseModel)
        {
            int purchaseInvoiceRouteId = 3;
            using(var unitOfWork = UnitOfWorkManager.Begin())
            {
                _purchaseRepo = unitOfWork.GetRepository<Purchase>();

                OrganizationClient orgClient = new OrganizationClient(InternalAPICallService.JWTForInternal);
                await orgClient.AllowedOrganizationsAsync($"GetAllowedOrganizationsToUserByRoute/{User.UserId}/{purchaseInvoiceRouteId}");
                //_purchaseRepo.UnTrackableQuery().Where(x => x.)
                throw new NotImplementedException();
            }
        }

        public Task RemoveInvoiceAsync(int id) => throw new NotImplementedException();

        public Task<PurchaseModel> UpdateExistingInvoiceAsync(PurchaseModel purchaseModel) => throw new NotImplementedException();
    }
}
