using Inventory.Data.Entities;
using Inventory.Data.Models.Product;
using Inventory.Data.Models.Purchase;
using Inventory.Domain.Abstraction.Sales;
using Inventory.Domain.Implementation.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Domain;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Inventory.Domain.Implementation.Sales
{
    public class InventoryService : ServiceBase, IInventoryService
    {
        private IHttpContextAccessor _contextAccessor;
        private IClaimsPrincipalAccessor _claimsPrincipalAccessor;
        public InventoryService(
            ILogger<ProductService> logger,
            IObjectMapper mapper,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IUnitOfWorkManager unitOfWork,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
        }

        public async Task<IEnumerable<PurchaseInvoiceDetailsModel>> GetInventoryOfVariant(int productVariantId)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _purchaseRepo = _repositoryFactory.GetRepository<PurchaseInvoiceDetails>();

                return await _purchaseRepo.UnTrackableQuery()
                    .Where(purchaseDetails => 
                        purchaseDetails.ProductVariantId == productVariantId
                        && purchaseDetails.CurrentStockQuantity > 0
                    )
                    .Select(purchaseDetails => new PurchaseInvoiceDetailsModel
                    {
                        RecordId = purchaseDetails.RecordId,
                        ProductVariantId = purchaseDetails.ProductVariantId,
                        ProductVariantName = purchaseDetails.ProductVariantName,
                        Quantity = purchaseDetails.CurrentStockQuantity,
                        RetailPrice = purchaseDetails.RetailPrice,
                        MaxRetailDiscount = purchaseDetails.MaxRetailDiscount
                    }).ToListAsync();
            }
        }
    }
}
