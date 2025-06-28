using Inventory.Data.Entities;
using Inventory.Data.Models.CRM;
using Inventory.Data.Models.Sales;
using Inventory.Domain.Abstraction;
using Inventory.Domain.Abstraction.CRM;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Domain;
using RedBook.Core.Models;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Inventory.Domain.Implementation.Sales
{
    public class SalesService: ServiceBase, ISalesService
    {
        private readonly ICustomerServices _customerServices;
        public SalesService(
            ILogger<SalesService> logger,
            IObjectMapper mapper,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IUnitOfWorkManager unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            ICustomerServices customerServices
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor)
        {
            _customerServices = customerServices;
        }

        public Task<SalesInvoiceModel> GetById(int invoiceId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedModel<SalesInvoiceModel>> GetPaged(PagedModel<SalesInvoiceModel> pagedInvoice)
        {
            using (var repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var salesInvoiceRepo = repositoryFactory.GetRepository<SalesInvoice>();
                var customerRepo = repositoryFactory.GetRepository<Customer>();
                var customerDetailRepo = repositoryFactory.GetRepository<CustomerDetails>();
                var purchaseInvoiceDetailRepo = repositoryFactory.GetRepository<PurchaseInvoiceDetails>();

                var baseQuery = salesInvoiceRepo.UnTrackableQuery()
                    .Where(salesInvoice => salesInvoice.OrganizationId == pagedInvoice.OrganizationId)
                    .GroupJoin(
                        customerRepo.UnTrackableQuery(),
                        salesInvoice => salesInvoice.CustomerId,
                        customer => customer.CustomerId,
                        (salesInvoice, matchingCustomers) => new { salesInvoice, matchingCustomers }
                    )
                    .SelectMany(
                        joined => joined.matchingCustomers.DefaultIfEmpty(),
                        (joined, customer) => new { joined.salesInvoice, customer }
                    )
                    .GroupJoin(
                        customerDetailRepo.UnTrackableQuery(),
                        joined => joined.customer != null ? joined.customer.CustomerId : 0,
                        customerDetail => customerDetail.CustomerId,
                        (joined, matchingDetails) => new { joined.salesInvoice, joined.customer, matchingDetails }
                    )
                    .SelectMany(
                        joined => joined.matchingDetails.DefaultIfEmpty(),
                        (joined, customerDetail) => new { joined.salesInvoice, joined.customer, customerDetail }
                    )
                    .GroupJoin(
                        purchaseInvoiceDetailRepo.UnTrackableQuery(),
                        joined => joined.salesInvoice.InvoiceId,
                        detail => detail.RecordId,
                        (joined, matchingDetails) => new { joined.salesInvoice, joined.customer, joined.customerDetail, matchingDetails }
                    )
                    .SelectMany(
                        joined => joined.matchingDetails.DefaultIfEmpty(),
                        (joined, purchaseInvoiceDetail) => new
                        {
                            SalesInvoice = joined.salesInvoice,
                            Customer = joined.customer,
                            CustomerDetail = joined.customerDetail,
                            PurchaseInvoiceDetail = purchaseInvoiceDetail
                        }
                    );

                if (!string.IsNullOrWhiteSpace(pagedInvoice.SearchString))
                {
                    string search = pagedInvoice.SearchString.Trim();

                    baseQuery = baseQuery.Where(x =>
                        (x.SalesInvoice.InvoiceNumber != null && x.SalesInvoice.InvoiceNumber.Contains(search)) ||
                        (x.CustomerDetail != null && x.CustomerDetail.CustomerName != null && x.CustomerDetail.CustomerName.Contains(search)) ||
                        (x.Customer != null && (
                            (x.Customer.Email != null && x.Customer.Email.Contains(search)) ||
                            (x.Customer.ContactNumber != null && x.Customer.ContactNumber.Contains(search))
                        )) ||
                        (x.PurchaseInvoiceDetail != null && x.PurchaseInvoiceDetail.BarCode != null && x.PurchaseInvoiceDetail.BarCode.Contains(search))
                    );
                }

                int totalRecords = await baseQuery.CountAsync();

                var pagedResults = await baseQuery
                    .Select(x => new SalesInvoiceModel
                    {
                        InvoiceId = x.SalesInvoice.InvoiceId,
                        InvoiceNumber = x.SalesInvoice.InvoiceNumber,
                        Customer = new CustomerModel
                        {
                            CustomerId = x.Customer == null? 0 : x.Customer.CustomerId,
                            CustomerName = x.CustomerDetail == null? "Guest Customer" : x.CustomerDetail.CustomerName,
                            Email = x.Customer == null ? string.Empty : x.Customer.Email,
                            ContactNumber = x.Customer == null ? string.Empty : x.Customer.ContactNumber
                        },
                        SalesDate = x.SalesInvoice.CreateDate.ToLongDateString(),
                        InvoiceTotal = x.SalesInvoice.InvoiceTotal,
                        Terms = x.SalesInvoice.Terms,
                        Remarks = x.SalesInvoice.Remarks
                    })
                    .Skip(pagedInvoice.Skip)
                    .Take(pagedInvoice.PageLength)
                    .ToListAsync();

                pagedInvoice.SourceData = pagedResults;
                pagedInvoice.TotalItems = totalRecords;
                return pagedInvoice;
            }
        }


        public async Task<SalesInvoiceModel> Save(SalesInvoiceModel invoice)
        {
            using (var repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var salesRepository = repositoryFactory.GetRepository<SalesInvoice>();
                var lotRepository = repositoryFactory.GetRepository<PurchaseInvoiceDetails>();

                Customer? CustomerEnttity = null;
                if (invoice.Customer != null)
                {
                    invoice.Customer = await _customerServices.SyncCustomerInfoAsync(invoice.Customer);
                    CustomerEnttity = Mapper.Map<Customer>(invoice.Customer);
                }

                SalesInvoice invoiceMaster = new SalesInvoice
                {
                    CreateBy = User.UserId,
                    CreateDate = DateTime.UtcNow,
                    CustomerId = CustomerEnttity == null ? null : CustomerEnttity.CustomerId,
                    Customer = CustomerEnttity,
                    InvoiceNumber = Guid.NewGuid().ToString(),
                    InvoiceTotal = invoice.InvoiceTotal,
                    IsDeleted = false,
                    OrganizationId = invoice.OrganizationId,
                    Remarks = invoice.Remarks,
                    Terms = invoice.Terms,
                    TotalDiscount = invoice.TotalDiscount,
                    SalesInvoiceDetails = new List<SalesInvoiceDetails>()
                };

                decimal totalPrice = 0;
                Dictionary<int, PurchaseInvoiceDetails> inventoryStatus = await lotRepository.UnTrackableQuery()
                    .Where(lot => invoice.SalesDetails.Select(salesDetails => salesDetails.LotId).ToArray().Contains(lot.RecordId))
                    .Select(lot => new PurchaseInvoiceDetails {
                        RecordId = lot.RecordId,
                        CurrentStockQuantity = lot.CurrentStockQuantity,
                        RetailPrice = lot.RetailPrice,
                        MaxRetailDiscount = lot.MaxRetailDiscount
                    })
                    .ToDictionaryAsync(lot => lot.RecordId, lot => lot);
                foreach (SalesInvoiceDetailsModel details in invoice.SalesDetails)
                {
                    if (!inventoryStatus.ContainsKey(details.LotId))
                        throw new ArgumentException($"Lot ID: {details.LotId} not found for Product: {details.ProductName} Variant: {details.ProductVariantName}");

                    if (inventoryStatus[details.LotId].CurrentStockQuantity < details.Quantity)
                        throw new ArgumentException($"Insufficient stock for Lot ID: {details.LotId} for Product: {details.ProductName} Variant: {details.ProductVariantName}");

                    if (inventoryStatus[details.LotId].RetailPrice - inventoryStatus[details.LotId].MaxRetailDiscount < details.RetailPrice)
                        throw new ArgumentException($"Retail price for Lot ID: {details.LotId} for Product: {details.ProductName} Variant: {details.ProductVariantName} can not be bellow {inventoryStatus[details.LotId].RetailPrice - inventoryStatus[details.LotId].MaxRetailDiscount}");

                    if (details.TotalCostPrice != (details.RetailPrice * details.Quantity) + (details.VatRate - details.RetailDiscount))
                        throw new ArgumentException($"Total cost price for Lot ID: {details.LotId} for Product: {details.ProductName} Variant: {details.ProductVariantName} is not equal to the sum of retail price and vat amount");

                    SalesInvoiceDetails detailsEntity = new SalesInvoiceDetails
                    {
                        InvoiceId = invoiceMaster.InvoiceId,
                        VariantId = details.ProductVariantId,
                        ProductVariantName = details.ProductVariantName,
                        ProductName = details.ProductName,
                        PurchaseId = details.LotId,
                        Quantity = details.Quantity,
                        RetailDiscount = details.RetailDiscount,
                        RetailPrice = details.RetailPrice,
                        VatRate = details.VatRate,
                        VatAmount = details.RetailPrice * (details.Quantity * details.VatRate / 100),
                        CreateBy = User.UserId,
                        CreateDate = DateTime.UtcNow,
                    };

                    invoiceMaster.SalesInvoiceDetails.Add(detailsEntity);

                    totalPrice += details.TotalCostPrice;

                    inventoryStatus[details.LotId].CurrentStockQuantity -= detailsEntity.Quantity;

                    lotRepository.ColumnUpdate(details.LotId, new Dictionary<string, object>
                    {
                        { nameof(PurchaseInvoiceDetails.CurrentStockQuantity).ToString(), inventoryStatus[details.LotId].CurrentStockQuantity }
                    });
                }

                if (invoice.InvoiceTotal != totalPrice - invoice.TotalDiscount)
                    throw new ArgumentException("Invalid invoice total price");

                invoiceMaster = await salesRepository.InsertAsync(invoiceMaster);

                await salesRepository.SaveChangesAsync();

                return Mapper.Map<SalesInvoiceModel>(invoiceMaster);
            }
        }
    }
}
