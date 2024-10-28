using Inventory.Data.Entities;
using Inventory.Data.Models.Purchase;
using Inventory.Domain.Abstraction.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Domain;
using RedBook.Core.Models;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Inventory.Domain.Implementation.Purchase
{
    public class VendorService : ServiceBase, IVendorService
    {
        public VendorService(
            ILogger<VendorService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor) { }

        public async Task<VendorModel> AddNewAsync(VendorModel model)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _vendorRepo = factory.GetRepository<Vendor>();

                Vendor entity = Mapper.Map<Vendor>(model);

                await _vendorRepo.InsertAsync(entity);
                await _vendorRepo.SaveChangesAsync();

                model = Mapper.Map<VendorModel>(model);
                return model;
            }
        }

        public async Task<VendorModel> GetByIdAsync(int id)
        {
            // Must check ownership
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _vendorRepo = factory.GetRepository<Vendor>();

                Vendor? entity = await _vendorRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.VendorId == id);

                if (entity == null) throw new ArgumentException($"Vendor with identifier {id} was not found.");

                VendorModel model = Mapper.Map<VendorModel>(entity);

                return model;
            }
        }

        public async Task<IEnumerable<VendorModel>> GetByOrgId(int orgId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _vendorRepo = factory.GetRepository<Vendor>();

                return await _vendorRepo.UnTrackableQuery()
                    .Where(x => x.OrganizationId == orgId)
                    .Select(x => new VendorModel
                    {
                        VendorId = x.VendorId,
                        VendorName = x.VendorName,
                    })
                    .ToListAsync();
            }
        }

        public async Task<PagedModel<VendorModel>> GetPagedAsync(PagedModel<VendorModel> pagedModel)
        {
            using (IRepositoryFactory factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var query = factory.GetRepository<Vendor>().UnTrackableQuery();

                if (!string.IsNullOrEmpty(pagedModel.SearchString))
                    query = query.Where(vendor => vendor.VendorName.Contains(pagedModel.SearchString)
                    || vendor.PhoneNumber.Contains(pagedModel.SearchString)
                    || vendor.Address.Contains(pagedModel.SearchString)
                    || vendor.Remarks.Contains(pagedModel.SearchString));

                pagedModel.SourceData = await query.Where(vendor => vendor.OrganizationId == pagedModel.OrganizationId)
                    .Skip(pagedModel.Skip)
                    .Take(pagedModel.PageLength)
                    .Select(vendor => new VendorModel
                    {
                        VendorId = vendor.VendorId,
                        VendorName = vendor.VendorName,
                        ContactPerson = vendor.ContactPerson,
                        EmailAddress = vendor.EmailAddress,
                        PhoneNumber = vendor.PhoneNumber,
                        Address = vendor.Address
                    })
                    .ToListAsync();

                return pagedModel;
            }
        }

        public async Task<VendorModel> UpdateAsync(int id, Dictionary<string, object> updates)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var vendorRepo = factory.GetRepository<Vendor>();
                vendorRepo.ColumnUpdate(id, updates);
                await factory.SaveChangesAsync();
                return new VendorModel();
            }
        }

        public Task DeleteAsync(int id) => throw new NotImplementedException();
    }
}
