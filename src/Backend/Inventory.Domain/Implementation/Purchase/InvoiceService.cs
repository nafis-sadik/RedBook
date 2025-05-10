using Inventory.Data.Entities;
using Inventory.Data.Models.Purchase;
using Inventory.Domain.Abstraction.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Domain;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Inventory.Domain.Implementation.Purchase
{
    public class InvoiceService : ServiceBase, IInvoiceService
    {
        public InvoiceService(
            ILogger<InvoiceService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor) { }

        public async Task<PurchaseInvoiceModel> GetByIdAsync(int id)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _invoiceRepo = factory.GetRepository<PurchaseInvoice>();

                PurchaseInvoice? entity = await _invoiceRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.InvoiceId == id);
                if (entity == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);

                return Mapper.Map<PurchaseInvoiceModel>(entity);
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

                query = query.OrderByDescending(i => i.CreateDate)
                    .Skip(pagedModel.Skip)
                    .Take(pagedModel.PageLength);

                pagedModel.SourceData = await query.Select(i => new PurchaseInvoiceModel
                {
                    InvoiceId = i.InvoiceId,
                    ChalanNumber = i.ChalanNumber,
                    ChalanDate = i.ChalanDate,
                    InvoiceTotal = i.InvoiceTotal,
                    VendorName = i.Vendor == null? "" : i.Vendor.VendorName,
                    TotalPaid = i.PurchasePaymentRecords.Sum(pay => pay.PaymentAmount)
                }).ToListAsync();

                foreach(PurchaseInvoiceModel invoice in pagedModel.SourceData)
                {
                    if (invoice.TotalPaid >= invoice.InvoiceTotal)
                    {
                        invoice.PaymentStatus = "Paid";
                    }
                    else
                    {
                        if (invoice.InvoiceTotal == 0)
                            invoice.PaymentStatus = "Unpaid";
                        else
                            invoice.PaymentStatus = "Partially Paid";
                    }
                }

                return pagedModel;
            }
        }

        public async Task<PurchaseInvoiceModel> AddNewAsync(PurchaseInvoiceModel model)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _purchaseInvoiceRepo = factory.GetRepository<PurchaseInvoice>();
                // Validate uniqueness of purchase invoice
                bool inRecord = await _purchaseInvoiceRepo.UnTrackableQuery()
                    .AnyAsync(invoice => invoice.OrganizationId == model.OrganizationId && invoice.ChalanNumber.ToLower().Equals(model.ChalanNumber.ToLower()));
                if (inRecord)
                    throw new ArgumentException($"You already have an invoice with invoice id {model.ChalanNumber}");

                // Validate invoice accounting
                decimal invoiceTotal = 0; 
                foreach(PurchaseInvoiceDetailsModel invoiceDetails in model.PurchaseDetails) {
                    invoiceDetails.TotalPrice = (invoiceDetails.PurchasePrice * invoiceDetails.Quantity) - invoiceDetails.PurchaseDiscount;
                    invoiceTotal += invoiceDetails.TotalPrice;
                }
                invoiceTotal -= model.TotalDiscount;

                if (model.InvoiceTotal != invoiceTotal)
                    throw new ArgumentException($"Invoice total mismatched");

                // Insert master table record
                PurchaseInvoice purchaseMaster = Mapper.Map<PurchaseInvoice>(model);
                purchaseMaster.CreateDate = DateTime.UtcNow;
                purchaseMaster.CreateBy = User.UserId;

                foreach (PurchaseInvoiceDetails purchaseDetails in purchaseMaster.Purchases) {
                    purchaseDetails.CreateBy = User.UserId;
                    purchaseDetails.CreateDate = DateTime.UtcNow;
                    purchaseDetails.BarCode = Guid.NewGuid().ToString();
                    purchaseDetails.CurrentStockQuantity = purchaseDetails.Quantity;
                }

                purchaseMaster = await _purchaseInvoiceRepo.InsertAsync(purchaseMaster);

                await factory.SaveChangesAsync();

                return Mapper.Map<PurchaseInvoiceModel>(purchaseMaster);
            }
        }

        public async Task<PurchaseInvoiceModel> UpdateAsync(int id, Dictionary<string, object> updates)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _purchaseInvoiceRepo = factory.GetRepository<PurchaseInvoice>();

                _purchaseInvoiceRepo.ColumnUpdate(id, updates);

                await factory.SaveChangesAsync();

                var entity = _purchaseInvoiceRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.InvoiceId == id);

                return Mapper.Map<PurchaseInvoiceModel>(entity);
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
