﻿using Identity.Data.CommonConstant;
using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Domain;
using RedBook.Core.Models;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Identity.Domain.Implementation
{
    public class EmployeeService : ServiceBase, IEmployeeService
    {
        public EmployeeService(
            ILogger<EmployeeService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor)
        { }

        public async Task<PagedModel<UserModel>> PagedEmployeeList(PagedModel<UserModel> pagedEmployeeList, int orgId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                if (!await this.IsAdminOf(_userRoleRepo, orgId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                var query = _userRoleRepo.UnTrackableQuery()
                    .Where(x => x.OrganizationId == orgId);

                if (!string.IsNullOrEmpty(pagedEmployeeList.SearchString))
                {
                    query = query.Where(x =>
                                x.User.FirstName.ToLower().Contains(pagedEmployeeList.SearchString.ToLower())
                                || x.User.LastName.ToLower().Contains(pagedEmployeeList.SearchString.ToLower())
                                || x.User.UserName.ToLower().Contains(pagedEmployeeList.SearchString.ToLower())
                                || x.Role.RoleName.ToLower().Contains(pagedEmployeeList.SearchString.ToLower())
                        );
                }

                pagedEmployeeList.SourceData = await query
                    .Skip(pagedEmployeeList.Skip)
                    .Take(pagedEmployeeList.PageLength)
                    .GroupBy(x => x.UserId)
                    .Select(u => new UserModel
                    {
                        UserId = u.First().User.UserId,
                        FirstName = u.First().User.FirstName,
                        LastName = u.First().User.LastName,
                        UserName = u.First().User.UserName,
                        Email = u.First().User.Email,
                        PhoneNumber = u.First().User.PhoneNumber,
                        UserRoles = _userRoleRepo
                            .UnTrackableQuery()
                            .Where(userRole => userRole.UserId == u.Key && userRole.OrganizationId == orgId)
                            .Select(x => new RoleModel
                            {
                                RoleId = x.RoleId,
                                RoleName = x.Role.RoleName
                            })
                            .ToArray()
                    })
                    .ToListAsync();

                pagedEmployeeList.TotalItems = await query.GroupBy(x => x.UserId).CountAsync();
                pagedEmployeeList.SearchString = string.IsNullOrEmpty(pagedEmployeeList.SearchString) ? "" : pagedEmployeeList.SearchString;
                return pagedEmployeeList;
            }
        }

        public async Task<UserModel> RegisterEmployee(int orgId, UserModel userModel)
        {
            if (string.IsNullOrEmpty(userModel.Email))
                throw new ArgumentException("Email not provided");

            if (userModel.ApplicationId <= 0)
                throw new ArgumentException("Unknown application");

            if (string.IsNullOrWhiteSpace(userModel.PhoneNumber))
                throw new ArgumentException("Phone number not provided");

            User userEntity = null;
            // The user might or might not be an existing user of this platform
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _userRepo = factory.GetRepository<User>();
                var _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                userEntity = await _userRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.PhoneNumber == userModel.PhoneNumber);
                if (userEntity == null)
                {
                    userEntity = Mapper.Map<User>(userModel);
                    userEntity.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(CommonConstants.PasswordConfig.DefaultPassword);
                    userEntity.Status = true;
                    userEntity = await _userRepo.InsertAsync(userEntity);
                    await factory.SaveChangesAsync();
                }

                foreach (var role in userModel.UserRoles)
                {
                    await _userRoleRepo.InsertAsync(new UserRoleMapping
                    {
                        RoleId = role.RoleId,
                        UserId = userEntity.UserId,
                        OrganizationId = orgId,
                        CreateBy = User.UserId,
                        CreateDate = DateTime.UtcNow,

                    });
                    await factory.SaveChangesAsync();
                }
            }

            return Mapper.Map<UserModel>(userEntity);
        }

        public async Task<UserModel> UpdateEmployeeRoles(int orgId, UserModel userModel)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _userRepo = factory.GetRepository<User>();
                var _userRoleRepo = factory.GetRepository<UserRoleMapping>();
                var userEntity = await _userRepo.TrackableQuery().FirstOrDefaultAsync(x => x.UserId == userModel.UserId);

                if (userEntity == null) throw new ArgumentException("Invalid user exception");

                Dictionary<int, UserRoleMapping> dbRoleIdDict = await _userRoleRepo.UnTrackableQuery()
                    .Where(mapping => mapping.UserId == userModel.UserId && !mapping.Role.IsSystemAdmin)
                    .ToDictionaryAsync(x => x.RoleId);

                foreach (var userRole in userModel.UserRoles)
                {
                    if (!dbRoleIdDict.ContainsKey(userRole.RoleId))
                    {
                        await _userRoleRepo.InsertAsync(new UserRoleMapping
                        {
                            RoleId = userRole.RoleId,
                            UserId = userModel.UserId,
                            OrganizationId = orgId
                        });
                    }
                }

                HashSet<int> frontEndRoleIdDict = userModel.UserRoles.Select(x => x.RoleId).ToHashSet();
                foreach (KeyValuePair<int, UserRoleMapping> dbRole in dbRoleIdDict)
                {
                    if (!frontEndRoleIdDict.Contains(dbRole.Key))
                        await _userRoleRepo.DeleteAsync(dbRole.Value.UserRoleId);
                }

                await factory.SaveChangesAsync();

                return Mapper.Map<UserModel>(userModel);
            }
        }

        public async Task ReleaseEmployee(int userId, int orgId)
        {
            using(var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var _roleMappingRepo = factory.GetRepository<Role>();
                var _userRoleMappingRepo = factory.GetRepository<UserRoleMapping>();

                // Check if operation is being done by an admin user or not
                if (!await this.IsAdminOf(_userRoleMappingRepo, orgId) && !await this.IsOwnerOf(_userRoleMappingRepo, orgId))
                    throw new ArgumentException("You must be the owner or have admin priviledge.");

                await _userRoleMappingRepo.TrackableQuery()
                    .Where(userRoleMapping => userRoleMapping.OrganizationId == orgId)
                    .ForEachAsync(userRoleMapping =>
                    {
                        userRoleMapping.Status = false;
                        userRoleMapping.UpdateBy = User.UserId;
                        userRoleMapping.UpdateDate = DateTime.UtcNow;
                    });

                await factory.SaveChangesAsync();
            }
        }

        public async Task<List<OrganizationModel>> AdminOrg()
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var userRoleRepo = factory.GetRepository<UserRoleMapping>();

                return await userRoleRepo.UnTrackableQuery()
                    .Where(userRoleMapping => userRoleMapping.UserId == User.UserId && userRoleMapping.Role.IsAdmin)
                    .Select(userRoleMapping => new OrganizationModel
                    {
                        OrganizationId = userRoleMapping.OrganizationId,
                        OrganizationName = userRoleMapping.Organization.OrganizationName,
                    }).ToListAsync();
            }
        }
    }
}
