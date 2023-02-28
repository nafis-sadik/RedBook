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
using System.Data;

namespace Identity.Domain.Implementation
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryBase<User> _userRepo;
        private readonly IRepositoryBase<Role> _roleRepo;

        public UserService(
            ILogger<UserService> logger,
            IObjectMapper mapper,
            IUnitOfWork unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor)
        {
            _unitOfWork = unitOfWork;
            _userRepo = _unitOfWork.GetRepository<User>();
            _roleRepo = _unitOfWork.GetRepository<Role>();
        }

        // Public API
        public async Task<string?> LogInAsync(UserModel userModel)
        {
            string userName = userModel.UserName, pass = userModel.Password;
            User? userEntity = await _userRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.UserName == userModel.UserName || x.Id == userModel.UserId);
            
            if (userEntity == null)
                return null;

            byte[] tokenKey = Encoding.ASCII.GetBytes(CommonConstants.PasswordConfig.Salt);
            if (userEntity != null && BCrypt.Net.BCrypt.EnhancedVerify(userModel.Password, userEntity.Password))
            {
                var roleEntity = await _roleRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.Id == userEntity.RoleId);

                return GenerateJwtToken(new List<Claim> {
                    new Claim("UserId", userEntity.Id),
                    new Claim(ClaimTypes.Role, roleEntity?.Id.ToString()),
                    new Claim("OrganizationId", userEntity?.OrganizationId.ToString())
                });
            }
            else
                return "";
        }
        public async Task<string?> SignUpAsync(UserModel userModel)
        {
            User? existingUser = await _userRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.UserName == userModel.UserName);

            if (existingUser != null)
                return null;

            using (var transaction = _unitOfWork.Begin())
            {
                User? userEntity = await _userRepo.InsertAsync(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    UserName = string.IsNullOrEmpty(userModel.UserName) ? userModel.FirstName + userModel.LastName : userModel.UserName,
                    Status = CommonConstants.StatusTypes.Active,
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword(userModel.Password),
                    AccountBalance = CommonConstants.DefaultCreditBalance,
                    OrganizationId = null,
                    RoleId = null
                });

                await transaction.SaveChangesAsync();

                Role? roleEntity;
                if (userEntity.RoleId != null)
                {
                    roleEntity = await _roleRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.Id == userEntity.RoleId);

                    return GenerateJwtToken(new List<Claim>
                    {
                        new Claim("UserId", userEntity.Id),
                        new Claim(ClaimTypes.Role, roleEntity?.Id.ToString()),
                        new Claim("OrganizationId", userEntity?.OrganizationId.ToString())
                    });
                }

                return GenerateJwtToken(new List<Claim> { new Claim("UserId", userEntity.Id) });
            }
        }

        // All User API
        public async Task<UserModel> UpdateOwnInformation(UserModel userModel)
        {
            var requestSenderId = User?.Claims.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value;
            if (userModel.UserId != requestSenderId) throw new ArgumentException($"User {requestSenderId} is not allowed to update information of {userModel.UserId}");

            User? userEntity = await _userRepo.GetByIdAsync(userModel.UserId);

            using (var transaction = _unitOfWork.Begin())
            {
                userEntity.FirstName = userModel.FirstName;
                userEntity.LastName = userModel.LastName;
                userEntity.UserName = userModel.UserName;
                userEntity.OrganizationId = userModel.OrganizationId;

                userEntity = _userRepo.Update(userEntity);

                await transaction.SaveChangesAsync();
            }

            return Mapper.Map<UserModel>(userEntity);
        }
        public async Task<UserModel?> GetOwnInformation()
        {
            var userId = User?.Claims.FirstOrDefault(x => x.Type.Equals("UserId", StringComparison.InvariantCultureIgnoreCase))?.Value;
            if(string.IsNullOrEmpty(userId)) throw new ArgumentException($"{nameof(userId)} is not valid user");
            User? userEntity = await _userRepo.GetByIdAsync(userId);
            return Mapper.Map<UserModel>(userEntity);
        }
        public async Task<bool> ArchiveAccount()
        {
            var userId = User?.Claims.FirstOrDefault(x => x.Type.Equals("UserId", StringComparison.InvariantCultureIgnoreCase))?.Value;

            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("Invalid User Id");

            using (var transaction = _unitOfWork.Begin())
            {
                User? userEntity = await _userRepo.GetByIdAsync(userId);
                userEntity.Status = CommonConstants.StatusTypes.Archived;
                _userRepo.Update(userEntity);
                await transaction.SaveChangesAsync();
            }

            return true;
        }

        // Organization Admin API
        public async Task<UserModel> RegisterNewUser(UserModel userModel)
        {
            int roleId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value);
            int orgId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals("OrganizationId", StringComparison.InvariantCultureIgnoreCase))?.Value);

            if (roleId != CommonConstants.GenericRoles.SystemAdminRoleId && roleId != CommonConstants.GenericRoles.AdminRoleId)
                throw new ArgumentException($"Only admin user can add new user to his organization");

            var newUser = Mapper.Map<User>(userModel);

            using (var transaction = _unitOfWork.Begin())
            {
                newUser.Id = new Guid().ToString();
                newUser.Password = BCrypt.Net.BCrypt.EnhancedHashPassword("12345678");
                newUser.OrganizationId = orgId;
                newUser.RoleId = userModel.RoleId;
                newUser = await _userRepo.InsertAsync(newUser);

                await transaction.SaveChangesAsync();
            }

            return Mapper.Map<UserModel>(newUser);
        }
        public async Task<bool> ResetPassword(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("Invalid User Id");

            var role = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;

            // Role Id for System Admin should always be 1
            // Only System Admin user can unarchive an user
            if (role != CommonConstants.GenericRoles.SystemAdminRoleId.ToString()) return false;

            using (var transaction = _unitOfWork.Begin())
            {
                User? userEntity = await _userRepo.GetByIdAsync(userId);
                userEntity.Password = BCrypt.Net.BCrypt.EnhancedHashPassword("12345678");
                _userRepo.Update(userEntity);
                await transaction.SaveChangesAsync();
            }

            return true;
        }

        // System Admin API
        public async Task<bool> DeleteAccount(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("Invalid User Id");

            var role = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;

            // Role Id for System Admin should always be 1
            // Only System Admin user can unarchive an user
            if (role != CommonConstants.GenericRoles.SystemAdminRoleId.ToString()) return false;

            using (var transaction = _unitOfWork.Begin())
            {
                User? userEntity = await _userRepo.GetByIdAsync(userId);
                userEntity.Status = CommonConstants.StatusTypes.Active;
                await _userRepo.DeleteAsync(userEntity);
                await transaction.SaveChangesAsync();
            }

            return true;
        }
        public async Task<bool> UnArchiveAccount(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("Invalid User Id");

            var role = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;

            // Role Id for System Admin should always be 1
            // Only System Admin user can unarchive an user
            if (role != CommonConstants.GenericRoles.SystemAdminRoleId.ToString()) return false;

            using (var transaction = _unitOfWork.Begin())
            {
                User? userEntity = await _userRepo.GetByIdAsync(userId);
                userEntity.Status = CommonConstants.StatusTypes.Active;
                _userRepo.Update(userEntity);
                await transaction.SaveChangesAsync();
            }

            return true;
        }

        private string GenerateJwtToken(List<Claim> claimList)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] tokenKey = Encoding.ASCII.GetBytes(CommonConstants.PasswordConfig.Salt);

            SecurityToken token = new JwtSecurityToken
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

            return tokenHandler.WriteToken(token);
        }
    }
}
