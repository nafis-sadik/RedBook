using Inventory.Data.Entities;
using Inventory.Data.Models.Purchase;
using Inventory.Domain.Abstraction.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
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
        public Task<PagedModel<VendorModel>> GetPagedAsync(PagedModel<VendorModel> pagedModel) => throw new NotImplementedException();
        public Task<VendorModel> UpdateAsync(int id, Dictionary<string, object> updates) => throw new NotImplementedException();
        public Task DeleteAsync(int id) => throw new NotImplementedException();
    }
}
