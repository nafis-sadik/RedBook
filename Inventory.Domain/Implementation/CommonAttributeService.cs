﻿using Inventory.Data.Entities;
using Inventory.Data.Models;
using Inventory.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Domain;
using RedBook.Core.EntityFramework;
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

        public async Task<CommonAttributeModel> AddCommonAttributeAsync(CommonAttributeModel commonAttrModel)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _commonAttrRepo = unitOfWork.GetRepository<CommonAttribute>();

                CommonAttribute commonAttr = await _commonAttrRepo.InsertAsync(new CommonAttribute
                {
                    AttributeName = commonAttrModel.AttributeName,
                    AttributeType = commonAttrModel.AttributeType,
                    CreateDate = DateTime.UtcNow,
                    CreatedBy = User.UserId,
                });

                await unitOfWork.SaveChangesAsync();

                return Mapper.Map<CommonAttributeModel>(commonAttr);
            }
        }
        public async Task DeleteCommonAttributeAsync(int attributeId)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _commonAttrRepo = unitOfWork.GetRepository<CommonAttribute>();
                await _commonAttrRepo.DeleteAsync(attributeId);
                await unitOfWork.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<CommonAttributeModel>> GetByTypeAsync(string attributeType)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _commonAttrRepo = unitOfWork.GetRepository<CommonAttribute>();

                return await _commonAttrRepo.UnTrackableQuery().Where(x => x.AttributeType == attributeType)
                    .Select(x => new CommonAttributeModel
                    {
                        AttributeId = x.AttributeId,
                        AttributeName = x.AttributeName,
                        AttributeType = x.AttributeType,
                    }).ToListAsync();
            }
        }
        public async Task<CommonAttributeModel> UpdateCommonAttributeAsync(CommonAttributeModel CommonAttributeModel)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _commonAttrRepo = unitOfWork.GetRepository<CommonAttribute>();

                CommonAttribute? commonAttribute = await _commonAttrRepo.GetAsync(CommonAttributeModel.AttributeId);
                if (commonAttribute == null) throw new ArgumentException("Resource not found");

                commonAttribute.AttributeName = CommonAttributeModel.AttributeName;
                commonAttribute.UpdateDate = DateTime.UtcNow;
                commonAttribute.UpdateBy = User.UserId;

                await _commonAttrRepo.SaveChangesAsync();

                return Mapper.Map<CommonAttributeModel>(commonAttribute);
            }
        }
    }
}
