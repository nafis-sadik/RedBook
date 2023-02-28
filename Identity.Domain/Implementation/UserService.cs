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
                    //Status = CommonConstants.StatusTypes.Active,
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
        public Task<UserModel> UpdateOwnInformation(UserModel userModel) => throw new NotImplementedException();
        public async Task<UserModel?> GetOwnInformation(string userId)
        {
            if (userId != User?.Claims.FirstOrDefault(x => x.Type.Equals("UserId", StringComparison.InvariantCultureIgnoreCase))?.Value) return null;
            User? userEntity = await _userRepo.GetByIdAsync(userId);
            return Mapper.Map<UserModel>(userEntity);
        }

        // Organization Admin API
        public Task<UserModel> RegisterNewUser(UserModel userModel) => throw new NotImplementedException();
        public async Task<bool> ArchiveAccount(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("Invalid User Id");

            var role = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;

            // Role Id for System Admin should always be 1
            if (role != 1.ToString()) return false;

            using (var transaction = _unitOfWork.Begin())
            {
                User? userEntity = await _userRepo.GetByIdAsync(userId);
                userEntity.Status = CommonConstants.StatusTypes.Archived;
                _userRepo.Update(userEntity);
                await transaction.SaveChangesAsync();
            }

            return true;
        }
        public async Task<bool> UnArchiveAccount(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("Invalid User Id");

            var role = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;

            // Role Id for System Admin should always be 1
            if (role != 1.ToString()) return false;

            using (var transaction = _unitOfWork.Begin())
            {
                User? userEntity = await _userRepo.GetByIdAsync(userId);
                userEntity.Status = CommonConstants.StatusTypes.Active;
                _userRepo.Update(userEntity);
                await transaction.SaveChangesAsync();
            }

            return true;
        }
        public Task<bool> ResetPassword(string userId) => throw new NotImplementedException();

        // System Admin API
        public Task<bool> DeleteAccount(string userId) => throw new NotImplementedException();
        
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

        //public bool DeleteAccount(string userId)
        //{
        //    try
        //    {
        //        User user = _userRepo.AsQueryable().FirstOrDefault(x => x.UserId == userId);
        //        if (user != null)
        //        {
        //            user.Status = CommonConstants.StatusTypes.Archived;
        //            _userRepo.Delete(user);
        //            return true;
        //        }

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        int pk = _crashLogRepo.AsQueryable().Count() + 1;

        //        _crashLogRepo.Add(new Crashlog
        //        {
        //            CrashLogId = pk,
        //            ClassName = "UserService",
        //            MethodName = "DeleteAccount",
        //            ErrorMessage = ex.Message,
        //            ErrorInner = (string.IsNullOrEmpty(ex.Message) || ex.Message == CommonConstants.MsgInInnerException ? ex.InnerException.Message : ex.Message),
        //            Data = userId,
        //            TimeStamp = DateTime.Now
        //        });
        //        return false;
        //    }
        //}

        //public bool ResetPassword(string userId)
        //{
        //    try
        //    {
        //        User userData = _userRepo.AsQueryable().FirstOrDefault(x => x.UserId == userId);
        //        if (userData != null)
        //        {
        //            userData.Password = BCrypt.Net.BCrypt.EnhancedHashPassword("ADIBA<3nafis");
        //            _userRepo.Update(userData);
        //            return true;
        //        }

        //        return false;
        //    } 
        //    catch (Exception ex)
        //    {
        //        int pk = _crashLogRepo.AsQueryable().Count() + 1;   

        //        _crashLogRepo.Add(new Crashlog
        //        {
        //            CrashLogId = pk,
        //            ClassName = "UserService",
        //            MethodName = "ResetPassword",
        //            ErrorMessage = ex.Message,
        //            ErrorInner = (string.IsNullOrEmpty(ex.Message) || ex.Message == CommonConstants.MsgInInnerException ? ex.InnerException.Message : ex.Message),
        //            Data = userId,
        //            TimeStamp = DateTime.Now
        //        });
        //        return false;
        //    }
        //}
    }
}
