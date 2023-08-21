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
        private IRepositoryBase<User> _userRepo;
        private IRepositoryBase<Role> _roleRepo;

        public UserService(
            ILogger<UserService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        { }

        // Public API
        public async Task<string?> LogInAsync(UserModel userModel)
        {
            string userName = userModel.UserName, pass = userModel.Password;

            User? userEntity;
            Role? roleEntity;
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _userRepo = unitOfWork.GetRepository<User>();
                _roleRepo = unitOfWork.GetRepository<Role>();
                userEntity = await _userRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.UserName == userModel.UserName || x.UserId == userModel.UserId);
                if(userEntity == null) throw new ArgumentException("User not found");
                roleEntity = await _roleRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.RoleId == userEntity.RoleId);
                if (roleEntity == null) throw new ArgumentException("User must have a role");
            }

            if (userEntity == null)
                return null;

            if (userEntity != null && BCrypt.Net.BCrypt.EnhancedVerify(userModel.Password, userEntity.Password))
            {
                byte[] tokenKey = Encoding.ASCII.GetBytes(CommonConstants.PasswordConfig.Salt);
                return GenerateJwtToken(new List<Claim> {
                    new Claim("UserId", userEntity.UserId),
                    new Claim(ClaimTypes.Role, roleEntity.RoleId.ToString()),
                    new Claim("OrganizationId", userEntity.OrganizationId.ToString())
                });
            }
            else
                return "";
        }
        
        public async Task<string?> SignUpAsync(UserModel userModel)
        {
            throw new NotImplementedException();
            //User? existingUser = await _userRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.UserName == userModel.UserName);

            //if (existingUser != null)
            //    return null;

            //using (var transaction = UnitOfWorkManager.Begin())
            //{
            //    User? userEntity = await _userRepo.InsertAsync(new User
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        FirstName = userModel.FirstName,
            //        LastName = userModel.LastName,
            //        UserName = string.IsNullOrEmpty(userModel.UserName) ? userModel.FirstName + userModel.LastName : userModel.UserName,
            //        Status = CommonConstants.StatusTypes.Active.ToString(),
            //        Password = BCrypt.Net.BCrypt.EnhancedHashPassword(userModel.Password),
            //        AccountBalance = CommonConstants.DefaultCreditBalance,
            //        OrganizationId = null,
            //        RoleId = null
            //    });

            //    await transaction.SaveChangesAsync();

            //    Role? roleEntity;

            //    roleEntity = await _roleRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.Id == userEntity.RoleId);

            //    return GenerateJwtToken(new List<Claim>
            //        {
            //            new Claim("UserId", userEntity.Id),
            //            new Claim(ClaimTypes.Role, roleEntity?.Id.ToString()),
            //            new Claim("OrganizationId", userEntity?.OrganizationId.ToString())
            //        });
            //}
        }

        // User API
        public async Task<UserModel> UpdateOwnInformation(UserModel userModel)
        {
            var requestSenderId = User?.Claims.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value;
            if (userModel.UserId != requestSenderId) throw new ArgumentException($"User {requestSenderId} is not allowed to update information of {userModel.UserId}");

            User? userEntity = await _userRepo.GetByIdAsync(userModel.UserId);

            using (var transaction = UnitOfWorkManager.Begin())
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

            using (var transaction = UnitOfWorkManager.Begin())
            {
                User? userEntity = await _userRepo.GetByIdAsync(userId);
                userEntity.Status = CommonConstants.StatusTypes.Archived.ToString();
                _userRepo.Update(userEntity);
                await transaction.SaveChangesAsync();
            }

            return true;
        }

        // Organization Admin API
        public async Task<UserModel> RegisterNewUser(UserModel userModel)
        {
            var requestSenderId = User?.Claims.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value;
            
            User newUser;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _userRepo = transaction.GetRepository<User>();

                User? requestSenderUser = await _userRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.UserId == requestSenderId);

                if (requestSenderUser == null)
                    throw new ArgumentException($"User {requestSenderId} was not found");

                int? senderRoleId = requestSenderUser?.RoleId;

                if (senderRoleId == null)
                    throw new ArgumentException($"User {requestSenderId} has no role assigned or doesn't exist");

                _roleRepo = transaction.GetRepository<Role>();

                Role? senderRole = await _roleRepo.GetByIdAsync(senderRoleId);

                if (senderRole == null)
                    throw new ArgumentException("Invalid sender role");

                if (senderRole.IsAdminRole == 0)
                    throw new ArgumentException($"Only admin user can add new user to his organization");

                int orgId = Convert.ToInt32(User?.Claims.FirstOrDefault(x => x.Type.Equals("OrganizationId", StringComparison.InvariantCultureIgnoreCase))?.Value);

                newUser = Mapper.Map<User>(userModel);
                
                newUser.UserId = Guid.NewGuid().ToString();
                newUser.Password = BCrypt.Net.BCrypt.EnhancedHashPassword("12345678");
                newUser.Status = CommonConstants.StatusTypes.Active.ToString();
                if(senderRole.RoleName.ToLower().Contains("system admin"))
                    newUser.OrganizationId = senderRole.RoleId;
                else
                    newUser.OrganizationId = orgId;

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

            using (var transaction = UnitOfWorkManager.Begin())
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

            using (var transaction = UnitOfWorkManager.Begin())
            {
                User? userEntity = await _userRepo.GetByIdAsync(userId);
                userEntity.Status = CommonConstants.StatusTypes.Active.ToString();
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

            using (var transaction = UnitOfWorkManager.Begin())
            {
                User? userEntity = await _userRepo.GetByIdAsync(userId);
                userEntity.Status = CommonConstants.StatusTypes.Active.ToString();
                _userRepo.Update(userEntity);
                await transaction.SaveChangesAsync();
            }

            return true;
        }
        public async Task<UserModel> RegisterAdminUser(UserModel userModel)
        {
            var role = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;
            if (!role.Equals(CommonConstants.GenericRoles.SystemAdminRoleId.ToString()))
                throw new ArgumentException($"Only System Admin users have access to execute this operation");

            var userEntity = new User
            {
                UserId = new Guid().ToString(),
                AccountBalance = 0,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                OrganizationId = userModel.OrganizationId,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword("OBO13nafu."),
                RoleId = CommonConstants.GenericRoles.AdminRoleId,
                Status = CommonConstants.StatusTypes.Active.ToString(),
                UserName = userModel.UserName
            };

            using(var transaction = UnitOfWorkManager.Begin())
            {
                userEntity = await _userRepo.InsertAsync(userEntity);
                await transaction.SaveChangesAsync();
            }
            return Mapper.Map<UserModel>(userEntity);
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
