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
            string userRoles = "";
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

                int[] userRolesIds = await _userRoleRepo.UnTrackableQuery()
                                .Where(x => x.UserId == userEntity.UserId)
                                .Select(x => x.RoleId)
                                .ToArrayAsync();


                userRoles = string.Join(",", userRolesIds);
            }

            if (userEntity != null && BCrypt.Net.BCrypt.EnhancedVerify(userModel.Password, userEntity.Password))
            {
                byte[] tokenKey = Encoding.ASCII.GetBytes(CommonConstants.PasswordConfig.Salt);
                return GenerateJwtToken(new List<Claim> {
                    new Claim("UserId", userEntity.UserId),
                    new Claim(ClaimTypes.Role, userRoles)
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

                if (User.UserId != userId)
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

        // Admin Only API
        public async Task<UserModel> RegisterNewUser(UserModel userModel)
        {
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _userRepo = transaction.GetRepository<User>();
                _roleRepo = transaction.GetRepository<Role>();
                _userRoleRepo = transaction.GetRepository<UserRole>();

                User newUser = Mapper.Map<User>(userModel);
                newUser.UserId = Guid.NewGuid().ToString();
                newUser.Password = BCrypt.Net.BCrypt.EnhancedHashPassword("12345678");
                newUser.Status = CommonConstants.StatusTypes.Active.ToString();

                newUser = await _userRepo.InsertAsync(newUser);

                await transaction.SaveChangesAsync();

                if (await this.HasRetailerPriviledge(_roleRepo))
                {
                    Role? AdminRole = await _roleRepo.GetAsync(userModel.OrganizationId);
                    if (AdminRole == null)
                    {
                        AdminRole = await _roleRepo.InsertAsync(new Role
                        {
                            IsAdmin = true,
                            IsRetailer = false,
                            RoleName = "Admin",
                            IsSystemAdmin = false,
                            OrganizationId = userModel.OrganizationId,
                        });

                        await _roleRepo.SaveChangesAsync();
                    }
                }


                UserRole userRole = await _userRoleRepo.InsertAsync(new UserRole {
                    RoleId = orgAdminRole.RoleId,
                    UserId = newUser.UserId,
                });

                await transaction.SaveChangesAsync();

                // Allow Org admin Routes
                var _routeRepo = transaction.GetRepository<Route>();
                var _routeRouteRepo = transaction.GetRepository<RoleRouteMapping>();
                var allRoutesOfApp = _routeRepo.UnTrackableQuery().Where(x => x.ApplicationId == userModel.ApplicationId);
                foreach (var route in allRoutesOfApp)
                {
                    await _routeRouteRepo.InsertAsync(new RoleRouteMapping
                    {
                        RoleId = orgAdminRole.RoleId,
                        RouteId = route.RouteId,
                    });
                }

                await transaction.SaveChangesAsync();
            }

            return Mapper.Map<UserModel>(newUser);
        }

        public async Task DeleteAccount(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("Invalid User Id");

            string? requesterRoleIdStr = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;
            if (string.IsNullOrEmpty(requesterRoleIdStr) || !int.TryParse(requesterRoleIdStr, out int requesterRoleId))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            string? requesterUserIdStr = User?.Claims.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value;
            if (string.IsNullOrEmpty(requesterUserIdStr) || !int.TryParse(requesterUserIdStr, out int requesterUserId))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            using (var transaction = UnitOfWorkManager.Begin())
            {
                _userRepo = transaction.GetRepository<User>();
                _roleRepo = transaction.GetRepository<Role>();

                User? userEntity = await _userRepo.GetAsync(userId);
                if (userEntity == null)
                    throw new ArgumentException($"User with identifier {userId} was not found");

                // Role Id for System Admin should always be 1
                // Only System Admin user can unarchive an user
                var requesterRoleEntity = await _roleRepo.GetAsync(requesterRoleId);
                if (requesterRoleEntity == null || requesterRoleEntity.IsAdmin)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                await _userRepo.DeleteAsync(userEntity);
                await transaction.SaveChangesAsync();
            }
        }

        public async Task UnArchiveAccount(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("Invalid User Id");

            string? requesterRoleIdStr = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;
            if (string.IsNullOrEmpty(requesterRoleIdStr) || !int.TryParse(requesterRoleIdStr, out int requesterRoleId))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            string? requesterUserIdStr = User?.Claims.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value;
            if (string.IsNullOrEmpty(requesterUserIdStr) || !int.TryParse(requesterUserIdStr, out int requesterUserId))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            using (var transaction = UnitOfWorkManager.Begin())
            {
                _userRepo = transaction.GetRepository<User>();
                _roleRepo = transaction.GetRepository<Role>();

                User? userEntity = await _userRepo.GetAsync(userId);
                if (userEntity == null)
                    throw new ArgumentException($"User with identifier {userId} was not found");

                // Role Id for System Admin should always be 1
                // Only System Admin user can unarchive an user
                var requesterRoleEntity = await _roleRepo.GetAsync(requesterRoleId);
                if (requesterRoleEntity == null || requesterRoleEntity.IsAdmin)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

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
            if (!rememberMe)
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
