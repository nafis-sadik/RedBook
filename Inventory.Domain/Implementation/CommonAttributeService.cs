using Inventory.Data.Entities;
using Inventory.Data.Models;
using Inventory.Domain.Abstraction;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Domain;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Implementation
{
    public class CommonAttributeService : ServiceBase, ICommonAttributeService
    {
        private IRepositoryBase<CommonAttribute> _commonAttrRepo;

        public CommonAttributeService(
            ILogger<CommonAttributeService> logger,
            IObjectMapper mapper,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IUnitOfWorkManager unitOfWork
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork) { }

        public async Task<CommonAttributeModel> AddCategoryAsync(CommonAttributeModel commonAttrModel)
        {
            using(var unitOfWork = UnitOfWorkManager.Begin())
            {
                _commonAttrRepo = unitOfWork.GetRepository<CommonAttribute>();

                CommonAttribute commonAttr = await _commonAttrRepo.InsertAsync(new CommonAttribute
                {
                    AttributeName = commonAttrModel.AttributeName,
                    AttributeType = commonAttrModel.AttributeType,
                    CreateDate = DateTime.Now,
                    CreatedBy = User.UserId,
                });

                await unitOfWork.SaveChangesAsync();

                return Mapper.Map<CommonAttributeModel>(commonAttr);
            }
        }
        public Task DeleteCategoryAsync(int attributeId) => throw new NotImplementedException();
        public Task<IEnumerable<CommonAttributeModel>> GetByTypeAsync(string attributeType) => throw new NotImplementedException();
        public Task<CommonAttributeModel> UpdateCategoryAsync(CommonAttributeModel CommonAttributeModel) => throw new NotImplementedException();
    }
}
