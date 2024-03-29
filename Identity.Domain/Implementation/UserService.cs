﻿using System.Text;
using System.Security.Claims;
using Identity.Data.Models;
using RedBook.Core.Repositories;
using Identity.Data.Entities;
using RedBook.Core.Constants;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using RedBook.Core.Domain;
using RedBook.Core.AutoMapper;
using Microsoft.Extensions.Logging;
using RedBook.Core.UnitOfWork;
using Identity.Domain.Abstraction;
using RedBook.Core.Security;
using Microsoft.AspNetCore.Http;
using RedBook.Core.Models;

namespace Identity.Domain.Implementation
{
    public class UserService : ServiceBase, IUserService
    {
        private IRepositoryFactory _repositoryFactory;
        private IRepositoryBase<User> _userRepo;
        private IRepositoryBase<Role> _roleRepo;
        private IRepositoryBase<UserRoleMapping> _userRoleRepo;

        public UserService(
            ILogger<UserService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor)
        { }

        // Public API
        public async Task<string?> LogInAsync(UserModel userModel)
        {
            User? userEntity;
            int[] userRolesIds;
            string[] userRoleNames;
            using (_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _userRepo = _repositoryFactory.GetRepository<User>();
                _userRoleRepo = _repositoryFactory.GetRepository<UserRoleMapping>();

                // Find user
                userEntity = await _userRepo
                    .UnTrackableQuery()
                    .FirstOrDefaultAsync(x => 
                        x.UserName == userModel.UserName
                        || x.UserId == userModel.UserId
                        || x.Email == userModel.Email
                    );

                if(userEntity == null) throw new ArgumentException("User not found");

                IList<RoleModel> userRoles = await _userRoleRepo.UnTrackableQuery()
                                .Where(x => x.UserId == userEntity.UserId)
                                .Select(x => new RoleModel { RoleId = x.RoleId , RoleName = x.Role.RoleName})
                                .ToListAsync();

                userRolesIds = userRoles.Select(x => x.RoleId).Distinct().ToArray();
                userRoleNames = userRoles.Select(x => x.RoleName).Distinct().ToArray();
            }

            if (BCrypt.Net.BCrypt.EnhancedVerify(userModel.Password, userEntity.Password))
            {
                byte[] tokenKey = Encoding.ASCII.GetBytes(CommonConstants.PasswordConfig.Salt);
                return GenerateJwtToken(new List<Claim> {
                    new Claim("UserId", userEntity.UserId.ToString()),
                    new Claim("UserName", userEntity.UserName),
                    new Claim("UserRoles", string.Join(",", userRoleNames)),
                    new Claim("UserRoleIds", string.Join(",", userRolesIds))
                }, userModel.RememberMe);
            }
            else
                return "";
        }

        // User API
        public async Task<UserModel> UpdateAsync(UserModel userModel)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _userRepo = factory.GetRepository<User>();
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();
                User? userEntity = await _userRepo.GetAsync(userModel.UserId);

                if (userEntity == null)
                    throw new ArgumentException($"User with identifier {userModel.UserId} was not found");

                // Update own data
                if (userModel.UserId == User.UserId)
                {
                    Mapper.Map(userEntity, userModel);
                }
                // Update employe data
                else
                {
                    userEntity.FirstName = userModel.FirstName;
                    userEntity.LastName = userModel.LastName;
                    userEntity.UserName = userModel.UserName;

                    int[] userModelRoleIds = userModel.Roles.Select(x => x.RoleId).ToArray();
                    UserRoleMapping[] roleMappingRecords = await _userRoleRepo.UnTrackableQuery()
                        .Where(x => x.UserId == userModel.UserId && userModel.OrganizationId == x.Role.OrganizationId)
                        .Select(x => new UserRoleMapping {
                            RoleId = x.RoleId,
                            UserRoleId = x.UserRoleId,
                        })
                        .ToArrayAsync();

                    foreach(int roleId in userModelRoleIds)
                    {
                        if(roleMappingRecords.First(x => x.RoleId == roleId) == null)
                            await _userRoleRepo.InsertAsync(new UserRoleMapping
                            {
                                RoleId = roleId,
                                UserId = userModel.UserId,
                            });
                    }

                    foreach (UserRoleMapping role in roleMappingRecords)
                    {
                        if (!userModelRoleIds.Contains(role.RoleId))
                            _userRoleRepo.Delete(role);
                    }
                }

                userEntity = _userRepo.Update(userEntity);

                await factory.SaveChangesAsync();

                return Mapper.Map<UserModel>(userEntity);    
            }
        }

        public async Task<UserModel?> GetById(int userId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _userRepo = factory.GetRepository<User>();
                _roleRepo = factory.GetRepository<Role>();

                Role? roleEntity = await _roleRepo.GetAsync(userId);
                if (roleEntity == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);

                return Mapper.Map<UserModel>(roleEntity);
            }
        }

        public async Task<bool> ArchiveAccount(int userId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _userRepo = factory.GetRepository<User>();

                if (User.UserId == userId)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                User? userEntity = await _userRepo.GetAsync(userId);
                if (userEntity == null)
                    throw new ArgumentException($"User with identifier {userId} was not found");

                userEntity.Status = false;
                _userRepo.Update(userEntity);
                await factory.SaveChangesAsync();
            }

            return true;
        }

        public async Task ResetPassword(int userId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _userRepo = factory.GetRepository<User>();
                _roleRepo = factory.GetRepository<Role>();
                var _orgRepo = factory.GetRepository<Organization>();

                User? userEntity = await _userRepo.GetAsync(User.UserId);
                if (userEntity == null)
                    throw new ArgumentException($"User with identifier {userId} was not found");

                // Role Id for System Admin should always be 1
                // Only System Admin user can unarchive an user
                if (userId != User.UserId)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                userEntity.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(CommonConstants.PasswordConfig.DefaultPassword);
                _userRepo.Update(userEntity);

                await factory.SaveChangesAsync();
            }
        }

        // SysAdmin Only API
        public async Task<UserModel> RegisterNewUser(UserModel userModel)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _userRepo = factory.GetRepository<User>();
                _roleRepo = factory.GetRepository<Role>();
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                if (string.IsNullOrEmpty(userModel.Email))
                    throw new ArgumentException("Email not provided");

                User? newUser = await _userRepo.TrackableQuery().FirstOrDefaultAsync(x => userModel.Email.Equals(x.Email));
                if(newUser == null)
                {
                    newUser = Mapper.Map<User>(userModel);
                    newUser.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(CommonConstants.PasswordConfig.DefaultPassword);
                    newUser.Status = true;
                    newUser = await _userRepo.InsertAsync(newUser);
                    await factory.SaveChangesAsync();
                }

                Role? orgAdminRole = await _roleRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.OrganizationId == userModel.OrganizationId && x.IsAdmin == true);
                if (orgAdminRole == null)
                {
                    orgAdminRole = await _roleRepo.InsertAsync(new Role
                    {
                        IsAdmin = true,
                        IsRetailer = false,
                        RoleName = "Admin",
                        IsSystemAdmin = false,
                        OrganizationId = userModel.OrganizationId,
                    });

                    await _roleRepo.SaveChangesAsync();
                }

                UserRoleMapping userRole = await _userRoleRepo.InsertAsync(new UserRoleMapping {
                    RoleId = orgAdminRole.RoleId,
                    UserId = newUser.UserId,
                });

                await factory.SaveChangesAsync();

                // Allow Org admin Routes
                var _routeRepo = factory.GetRepository<Route>();
                var _roleRouteRepo = factory.GetRepository<RoleRouteMapping>();
                var allRoutesOfApp = _routeRepo.UnTrackableQuery().Where(x => x.ApplicationId == userModel.ApplicationId);

                foreach (var route in allRoutesOfApp)
                {
                    await _roleRouteRepo.InsertAsync(new RoleRouteMapping
                    {
                        RoleId = orgAdminRole.RoleId,
                        RouteId = route.RouteId,
                    });
                }

                await factory.SaveChangesAsync();

                return Mapper.Map<UserModel>(newUser);
            }
        }

        public async Task DeleteAccount(int userId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _userRepo = factory.GetRepository<User>();
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                if (! await this.HasSystemAdminPriviledge(_userRoleRepo))
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                await _userRepo.DeleteAsync(userId);
                await factory.SaveChangesAsync();
            }
        }

        public async Task UnArchiveAccount(int userId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _userRepo = factory.GetRepository<User>();
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                if (!await this.HasSystemAdminPriviledge(_userRoleRepo))
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                User? userEntity = await _userRepo.GetAsync(userId);
                if (userEntity == null)
                    throw new ArgumentException($"User with identifier {userId} was not found");

                userEntity.Status = true;
                _userRepo.Update(userEntity);
                await factory.SaveChangesAsync();
            }
        }

        private string GenerateJwtToken(List<Claim> claimList, bool rememberMe)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] tokenKey = Encoding.ASCII.GetBytes(CommonConstants.PasswordConfig.Salt);

            SecurityToken token;
            if (rememberMe)
            {
                token = new JwtSecurityToken
                (
                    issuer: "Blume.Id",
                    audience: "User",
                    expires: DateTime.UtcNow.AddDays(CommonConstants.PasswordConfig.SaltExpire),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature
                    ),
                    claims: claimList
                );
            }else
            {
                token = new JwtSecurityToken
                (
                    issuer: "Blume.Id",
                    audience: "User",
                    expires: DateTime.UtcNow.AddDays(DateTime.MaxValue.Day),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature
                    ),
                    claims: claimList
                );
            }

            return tokenHandler.WriteToken(token);
        }

        public async Task<UserModel> AddUserToBusiness(UserModel userModel)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _userRepo = factory.GetRepository<User>();
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                if (!await this.HasOwnerAdminPriviledge(_userRoleRepo, userModel.OrganizationId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                User? userEntity = await _userRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.Email == userModel.Email);
                if (userEntity == null)
                {
                    userEntity = await _userRepo.InsertAsync(new User
                    {
                        FirstName = userModel.FirstName,
                        LastName = userModel.LastName,
                        Email = userModel.Email,
                        UserName = userModel.UserName,
                        Address = userModel.Address,
                        PhoneNumber = userModel.PhoneNumber,
                        Status = true,
                        Password = BCrypt.Net.BCrypt.EnhancedHashPassword(CommonConstants.PasswordConfig.DefaultPassword)
                    });

                    await factory.SaveChangesAsync();
                }

                // Assigned role selectef from the front end dropdown
                foreach (RoleModel role in userModel.Roles)
                {
                    await _userRoleRepo.InsertAsync(new UserRoleMapping
                    {
                        RoleId = role.RoleId,
                        UserId = userEntity.UserId,
                    });
                }

                await factory.SaveChangesAsync();

                return Mapper.Map<UserModel>(userEntity);
            }
        }

        public async Task<PagedModel<UserModel>> GetUserByOrganizationId(PagedModel<UserModel> pagedModel, int orgId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                if (!await this.HasAdminPriviledge(_userRoleRepo, orgId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                var query = _userRoleRepo.UnTrackableQuery()
                    .Where(x => x.Role.OrganizationId == orgId);

                if (!string.IsNullOrEmpty(pagedModel.SearchString))
                {
                    query = query.Where(x =>
                                x.User.FirstName.ToLower().Contains(pagedModel.SearchString.ToLower())
                                || x.User.LastName.ToLower().Contains(pagedModel.SearchString.ToLower())
                                || x.User.UserName.ToLower().Contains(pagedModel.SearchString.ToLower())
                                || x.Role.RoleName.ToLower().Contains(pagedModel.SearchString.ToLower())
                        );
                }

                pagedModel.SourceData = await query
                    .Skip(pagedModel.Skip)
                    .Take(pagedModel.PageLength)
                    .Distinct()
                    .Select(u => new UserModel
                    {
                        UserId = u.User.UserId,
                        FirstName = u.User.FirstName,
                        LastName = u.User.LastName,
                        UserName = u.User.UserName,
                        Email = u.User.Email,
                        PhoneNumber = u.User.PhoneNumber,
                        Roles = _userRoleRepo
                                    .UnTrackableQuery()
                                    .Where(x => x.UserId == u.UserId && x.Role.OrganizationId == orgId)
                                    .Select(x => new RoleModel
                                    {
                                        RoleId = x.RoleId,
                                        RoleName = x.Role.RoleName
                                    })
                                    .ToArray()
                    })
                    .ToListAsync();

                pagedModel.TotalItems = await query.CountAsync();
                pagedModel.SearchString = pagedModel.SearchString == null || pagedModel.SearchString == "null" ? "" : pagedModel.SearchString;
                return pagedModel;
            }
        }

        public async Task RemoveUserFromOrganization(int userId, int orgId)
        {
            using (var factory = UnitOfWorkManager.GetRepositoryFactory())
            {
                _userRoleRepo = factory.GetRepository<UserRoleMapping>();

                if (!await this.HasAdminPriviledge(_userRoleRepo, orgId)) throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                await _userRoleRepo.DeleteAsync(x => x.UserId == userId && x.Role.OrganizationId == orgId && !x.Role.IsAdmin == true);

                await factory.SaveChangesAsync();
            }
        }
    }
}
