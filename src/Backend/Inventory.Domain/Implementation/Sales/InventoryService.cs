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

        public async Task<IEnumerable<VariantInventoryStatusModel>> GetProductInventory(int ProductId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var purchaseDetailsRepo = factory.GetRepository<PurchaseInvoiceDetails>();

                List<PurchaseInvoiceDetails> invoiceDetails = await purchaseDetailsRepo.UnTrackableQuery()
                    .Where(invoiceDetails => invoiceDetails.ProductVariant.ProductId == ProductId && invoiceDetails.CurrentStockQuantity > 0)
                    .Select(invoiceDetails => new PurchaseInvoiceDetails
                    {
                        InvoiceId = invoiceDetails.InvoiceId,
                        ProductVariantId = invoiceDetails.ProductVariantId,
                        ProductVariantName = invoiceDetails.ProductVariantName,
                        ProductName = invoiceDetails.ProductName,
                        PurchasePrice = invoiceDetails.PurchasePrice - (invoiceDetails.PurchaseDiscount / invoiceDetails.Quantity),
                        RetailPrice = invoiceDetails.RetailPrice,
                        MaxRetailDiscount = invoiceDetails.MaxRetailDiscount,
                        Quantity = invoiceDetails.Quantity,
                        CreateDate = invoiceDetails.CreateDate,
                        ProductVariant = new ProductVariant
                        {
                            ProductId = invoiceDetails.ProductVariant.ProductId,
                            SKU = invoiceDetails.ProductVariant.SKU
                        }
                    })
                    .ToListAsync();

                Dictionary<int, VariantInventoryStatusModel> inventory = new Dictionary<int, VariantInventoryStatusModel>();
                foreach (PurchaseInvoiceDetails invoice in invoiceDetails)
                {
                    if (!inventory.ContainsKey(invoice.InvoiceId))
                    {
                        inventory.Add(invoice.InvoiceId, new VariantInventoryStatusModel
                        {
                            ProductId = invoice.ProductVariant.ProductId,
                            ProductName = invoice.ProductName,
                            VariantId = invoice.ProductVariantId,
                            VariantName = invoice.ProductVariantName,
                            SKU = invoice.ProductVariant.SKU,
                            Lots = new List<PurchaseInvoiceDetailsModel>()
                        });
                    }
                    inventory[invoice.InvoiceId].Lots.Add(new PurchaseInvoiceDetailsModel
                    {
                        PurchaseDate = invoice.CreateDate,
                        PurchasePrice = invoice.PurchasePrice,
                        RetailPrice = invoice.RetailPrice,
                        MaxRetailDiscount = invoice.MaxRetailDiscount,
                        Quantity = invoice.Quantity,
                    });
                }

                return inventory.Values;
            }
        }
    }
}
