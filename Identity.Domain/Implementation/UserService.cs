using System.Text;
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
using System.Linq;

namespace Identity.Domain.Implementation
{
    public class UserService : ServiceBase, IUserService
    {
        private IRepositoryBase<User> _userRepo;
        private IRepositoryBase<Role> _roleRepo;
        private IRepositoryBase<UserRole> _userRoleRepo;

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
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _userRepo = unitOfWork.GetRepository<User>();
                _userRoleRepo = unitOfWork.GetRepository<UserRole>();

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
                    new Claim("UserId", userEntity.UserId),
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
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _userRepo = transaction.GetRepository<User>();
                _userRoleRepo = transaction.GetRepository<UserRole>();
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
                    UserRole[] roleMappingRecords = await _userRoleRepo.UnTrackableQuery()
                        .Where(x => x.UserId == userModel.UserId && userModel.OrganizationId == x.Role.OrganizationId)
                        .Select(x => new UserRole {
                            RoleId = x.RoleId,
                            UserRoleId = x.UserRoleId,
                        })
                        .ToArrayAsync();

                    foreach(int roleId in userModelRoleIds)
                    {
                        if(roleMappingRecords.First(x => x.RoleId == roleId) == null)
                            await _userRoleRepo.InsertAsync(new UserRole
                            {
                                RoleId = roleId,
                                UserId = userModel.UserId,
                            });
                    }

                    foreach (UserRole role in roleMappingRecords)
                    {
                        if (!userModelRoleIds.Contains(role.RoleId))
                            _userRoleRepo.Delete(role);
                    }
                }

                userEntity = _userRepo.Update(userEntity);

                await transaction.SaveChangesAsync();

                return Mapper.Map<UserModel>(userEntity);    
            }
        }

        public async Task<UserModel?> GetById(string userId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _userRepo = transaction.GetRepository<User>();
                _roleRepo = transaction.GetRepository<Role>();

                Role? roleEntity = await _roleRepo.GetAsync(userId);
                if (roleEntity == null) throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidInput);

                return Mapper.Map<UserModel>(roleEntity);
            }
        }

        public async Task<bool> ArchiveAccount(string userId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _userRepo = transaction.GetRepository<User>();

                if (User.UserId == userId)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                User? userEntity = await _userRepo.GetAsync(userId);
                if (userEntity == null)
                    throw new ArgumentException($"User with identifier {userId} was not found");

                userEntity.Status = CommonConstants.StatusTypes.Archived.ToString();
                _userRepo.Update(userEntity);
                await transaction.SaveChangesAsync();
            }

            return true;
        }

        public async Task ResetPassword(string userId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _userRepo = transaction.GetRepository<User>();
                _roleRepo = transaction.GetRepository<Role>();
                var _orgRepo = transaction.GetRepository<Organization>();

                User? userEntity = await _userRepo.GetAsync(User.UserId);
                if (userEntity == null)
                    throw new ArgumentException($"User with identifier {userId} was not found");

                // Role Id for System Admin should always be 1
                // Only System Admin user can unarchive an user
                if (userId != User.UserId)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                userEntity.Password = BCrypt.Net.BCrypt.EnhancedHashPassword("12345678");
                _userRepo.Update(userEntity);

                await transaction.SaveChangesAsync();
            }
        }

        // SysAdmin Only API
        public async Task<UserModel> RegisterNewUser(UserModel userModel)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _userRepo = transaction.GetRepository<User>();
                _roleRepo = transaction.GetRepository<Role>();
                _userRoleRepo = transaction.GetRepository<UserRole>();

                if (string.IsNullOrEmpty(userModel.Email))
                    throw new ArgumentException("Email not provided");

                User? newUser = await _userRepo.TrackableQuery().FirstOrDefaultAsync(x => userModel.Email.Equals(x.Email));
                if(newUser == null)
                {
                    newUser = Mapper.Map<User>(userModel);
                    newUser.UserId = Guid.NewGuid().ToString();
                    newUser.Password = BCrypt.Net.BCrypt.EnhancedHashPassword("12345678");
                    newUser.Status = CommonConstants.StatusTypes.Active.ToString();
                    newUser = await _userRepo.InsertAsync(newUser);
                    await transaction.SaveChangesAsync();
                }

                Role? orgAdminRole = await _roleRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.OrganizationId == userModel.OrganizationId && x.IsAdmin);
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

                UserRole userRole = await _userRoleRepo.InsertAsync(new UserRole {
                    RoleId = orgAdminRole.RoleId,
                    UserId = newUser.UserId,
                });

                await transaction.SaveChangesAsync();

                // Allow Org admin Routes
                var _routeRepo = transaction.GetRepository<Route>();
                var _roleRouteRepo = transaction.GetRepository<RoleRouteMapping>();
                var allRoutesOfApp = _routeRepo.UnTrackableQuery().Where(x => x.ApplicationId == userModel.ApplicationId);

                foreach (var route in allRoutesOfApp)
                {
                    await _roleRouteRepo.InsertAsync(new RoleRouteMapping
                    {
                        RoleId = orgAdminRole.RoleId,
                        RouteId = route.RouteId,
                    });
                }

                await transaction.SaveChangesAsync();

                return Mapper.Map<UserModel>(newUser);
            }
        }

        public async Task DeleteAccount(string userId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _userRepo = transaction.GetRepository<User>();
                _roleRepo = transaction.GetRepository<Role>();

                if (! await this.HasSystemAdminPriviledge(_roleRepo))
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                await _userRepo.DeleteAsync(userId);
                await transaction.SaveChangesAsync();
            }
        }

        public async Task UnArchiveAccount(string userId)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _userRepo = transaction.GetRepository<User>();
                _roleRepo = transaction.GetRepository<Role>();

                if (!await this.HasSystemAdminPriviledge(_roleRepo))
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                User? userEntity = await _userRepo.GetAsync(userId);
                if (userEntity == null)
                    throw new ArgumentException($"User with identifier {userId} was not found");

                userEntity.Status = CommonConstants.StatusTypes.Active.ToString();
                _userRepo.Update(userEntity);
                await transaction.SaveChangesAsync();
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
    }
}
