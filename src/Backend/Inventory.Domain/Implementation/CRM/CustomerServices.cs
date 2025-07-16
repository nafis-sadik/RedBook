using Inventory.Domain.Abstraction.CRM;
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
        public CustomerServices(
            ILogger<CustomerServices> logger,
            IObjectMapper mapper,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IUnitOfWorkManager unitOfWork,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor) { }

        public async Task<CustomerModel> SyncCustomerInfoAsync(CustomerModel model)
        {
            if (string.IsNullOrEmpty(model.ContactNumber) && string.IsNullOrEmpty(model.Email))
                throw new ArgumentException($"Your customer must provide contact number or email address");
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _customerRepo = _repositoryFactory.GetRepository<Customer>();
                var _customerDetailsRepo = _repositoryFactory.GetRepository<CustomerDetails>();

                int customerId = await _customerRepo.UnTrackableQuery()
                    .Where(customer => customer.ContactNumber.ToLower().Equals(model.ContactNumber.ToLower()))
                    .Select(customer => customer.CustomerId)
                    .FirstOrDefaultAsync();

                if (customerId <= 0)
                {
                    Customer customerEntity = Mapper.Map<Customer>(model);
                    customerEntity.CustomerDetails = [Mapper.Map<CustomerDetails>(model)];
                    customerEntity.CustomerId = 0;
                    foreach(CustomerDetails details in customerEntity.CustomerDetails)
                    {
                        details.CustomerDetailId = 0;
                        details.OrgId = model.OrgId;
                    }
                    customerEntity = await _customerRepo.InsertAsync(customerEntity);
                    await _repositoryFactory.SaveChangesAsync();
                    CustomerModel response = Mapper.Map<CustomerModel>(customerEntity);
                    response.OrgId = model.OrgId;
                    CustomerDetails? customerDetailsEntity = customerEntity.CustomerDetails.FirstOrDefault();
                    if(customerDetailsEntity != null)
                    {
                        response.CustomerName = customerDetailsEntity.CustomerName;
                        response.Address = customerDetailsEntity.Address;
                        response.Remarks = customerDetailsEntity.Remarks;
                    }

                    return response;
                }
                else 
                {
                    CustomerDetails? customerDetails = await _customerDetailsRepo.TrackableQuery()
                        .Where(customerDetails => 
                            customerDetails.CustomerId == customerId 
                            && customerDetails.OrgId == model.OrgId
                        ).FirstOrDefaultAsync();

                    if(customerDetails == null)
                    {
                        customerDetails = await _customerDetailsRepo.InsertAsync(new CustomerDetails
                        {
                            CustomerId = customerId,
                            OrgId = model.OrgId,
                            CustomerName = model.CustomerName,
                            Address = model.Address,
                            Remarks = model.Remarks,
                        });
                    }
                    else
                    {
                        customerDetails.CustomerName = model.CustomerName;
                        customerDetails.Address = model.Address;
                        customerDetails.Remarks = model.Remarks;
                    }

                    await _repositoryFactory.SaveChangesAsync();

                    return new CustomerModel
                    {
                        CustomerId = customerId,
                        OrgId = customerDetails.OrgId,
                        CustomerName = customerDetails.CustomerName,
                        ContactNumber = model.ContactNumber,
                        Email = model.Email,
                        Address = customerDetails.Address,
                        Remarks = customerDetails.Remarks,
                    };
                }
            }
        }

        public async Task<string[]> SearchByContactNumberFromPurchaseHistory(string searchString, int orgId)
        {
            searchString = searchString.Trim().Replace(" ", string.Empty);
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _customerDetailsRepo = _repositoryFactory.GetRepository<CustomerDetails>();

                // After implementation of gRPC, we need to verify if this user have the right role and the right
                var query = _customerDetailsRepo.UnTrackableQuery()
                    .Where(sales => sales.OrgId == orgId);
                searchString = searchString.Substring(1, searchString.Length - 1);
                if (searchString.Contains('@'))
                    query = query.Where(sales => sales.Customer.Email.ToLower().Contains(searchString.ToLower()));
                else
                    query = query.Where(sales => sales.Customer.ContactNumber.Contains(searchString));

                var nums = await query.Select(sales => sales.Customer.ContactNumber).ToArrayAsync();
                return nums;
            }
        }

        public async Task<CustomerModel?> FindCustomerByContactNumber(string searchString, int orgId)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _customerDetailsRepo = _repositoryFactory.GetRepository<CustomerDetails>();

                // After implementation of gRPC, we need to verify if this user have the right role and the right
                var query = _customerDetailsRepo.UnTrackableQuery().Where(customerDetails => customerDetails.OrgId == orgId);

                if (searchString.Contains('@'))
                    query = query.Where(sales => sales.Customer.Email.ToLower().Contains(searchString.ToLower()));

                else
                    query = query.Where(sales => sales.Customer.ContactNumber.Contains(searchString));

                var customer = await query.Select(customerDetails => new CustomerModel
                    {
                        CustomerId = customerDetails.CustomerId,
                        CustomerName = customerDetails.CustomerName,
                        Address = customerDetails.Address,
                        ContactNumber = customerDetails.Customer.ContactNumber,
                        Email = customerDetails.Customer.Email,
                        Remarks = customerDetails.Remarks,
                        OrgId = orgId
                    })
                    .FirstOrDefaultAsync();

                return customer;
            }
        }
    }
}
