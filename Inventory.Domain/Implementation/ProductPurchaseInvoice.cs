using Inventory.Clients;
using Inventory.Data.Entities;
using Inventory.Data.Models;
using Inventory.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Domain;
using RedBook.Core.Models;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using System.Linq;

namespace Inventory.Domain.Implementation
{
    public class ProductPurchaseInvoice : ServiceBase, IProductPurchaseInvoice
    {
        private IRepositoryFactory _repositoryFactory;
        private IRepositoryBase<Purchase> _purchaseRepo;
        private IRepositoryBase<PurchaseInvoice> _purchaseInvoiceRepo;
        public ProductPurchaseInvoice(
            ILogger<ProductPurchaseInvoice> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork) { }

        public Task AddNewInvoiceAsync(PurchaseModel purchaseModel) => throw new NotImplementedException();
        public async Task<PagedPurchaseInvoiceModel> GetPagedInvoiceAsync(PagedPurchaseInvoiceModel invoiceModel)
        {
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _purchaseRepo = _repositoryFactory.GetRepository<Purchase>();
                _purchaseInvoiceRepo = _repositoryFactory.GetRepository<PurchaseInvoice>();

                var query = _purchaseInvoiceRepo.UnTrackableQuery().Where(i => i.OrganizationId == invoiceModel.OrganizationId);

                if (!string.IsNullOrEmpty(invoiceModel.SearchString))
                    query = query.Where(i => i.ChalanNumber.ToLower().Contains(invoiceModel.SearchString));

                invoiceModel.TotalItems = await query.CountAsync();

                query = query.Skip(invoiceModel.Skip)
                    .Take(invoiceModel.PageLength)
                    .OrderByDescending(i => i.CreateDate);

                invoiceModel.SourceData = await query.Select(i => new PurchaseInvoiceModel
                {
                    InvoiceId = i.InvoiceId,
                    ChalanNumber = i.ChalanNumber,
                    TotalPurchasePrice = i.TotalPurchasePrice,
                    SearchString = invoiceModel.SearchString,
                    PageLength = invoiceModel.PageLength,
                    PageNumber = invoiceModel.PageNumber,
                    TotalItems = i.Purchases.Count(),
                    SourceData = i.Purchases.Select(p => new ProductModel { ProductId = p.ProductId, ProductName = p.Product.ProductName })
                }).ToListAsync();

                return invoiceModel;
            }
        }

        public Task RemoveInvoiceAsync(int id) => throw new NotImplementedException();

        public Task<PurchaseModel> UpdateExistingInvoiceAsync(PurchaseModel purchaseModel) => throw new NotImplementedException();
    }
}
