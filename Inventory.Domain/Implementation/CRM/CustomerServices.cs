using Inventory.Domain.Abstraction.CRM;
using Inventory.Domain.Implementation.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RedBook.Core.Domain;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using RedBook.Core.AutoMapper;
using Inventory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Inventory.Data.Models.CRM;

namespace Inventory.Domain.Implementation.CRM
{
    public class CustomerServices : ServiceBase, ICustomerServices
    {
        private IHttpContextAccessor _contextAccessor;
        private IClaimsPrincipalAccessor _claimsPrincipalAccessor;
        public CustomerServices(
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

        public async Task<string[]> SearchByContactNumberFromPurchaseHistory(string contactNumber, int orgId)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _salesRepo = _repositoryFactory.GetRepository<SalesInvoice>();

                // After implementation of gRPC, we need to verify if this user have the right role and the right

                string[] tempres = await _salesRepo.UnTrackableQuery().Where(sales =>
                        sales.OrganizationId == orgId
                        && sales.Customer != null
                        && sales.Customer.ContactNumber.ToLower().Contains(contactNumber.ToLower())
                    ).Select(sales => sales.Customer.ContactNumber).ToArrayAsync();

                return tempres.Length <= 0 ? new string[]
                {
                    "01715422084",
                    "01628301510",
                    "01780705708"
                } : tempres;
            }
        }

        public async Task<CustomerModel?> FindCustomerByContactNumber(string contactNumber, int orgId)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _customerRepo = _repositoryFactory.GetRepository<Customer>();
                var _salesRepo = _repositoryFactory.GetRepository<SalesInvoice>();

                // After implementation of gRPC, we need to verify if this user have the right role and the right

                return await _salesRepo.UnTrackableQuery()
                    .Where(sales =>
                        sales.OrganizationId == orgId
                        && sales.Customer != null && sales.CustomerId != null
                        && sales.Customer.ContactNumber.ToLower().Contains(contactNumber.ToLower())
                    ).Select(sales => new CustomerModel
                    {
                        CustomerId = sales.CustomerId.Value,
                        CustomerName = sales.Customer.CustomerName,
                        Address = sales.Customer.Address,
                        ContactNumber = sales.Customer.ContactNumber,
                        Email = sales.Customer.Email,
                        Remarks = sales.Customer.Remarks
                    })
                    .FirstOrDefaultAsync();

            }
        }
    }
}
