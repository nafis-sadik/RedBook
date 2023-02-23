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
            IRepositoryBase<Role> roleRepo,
            IRepositoryBase<User> userRepo,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor)
        {
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
            _roleRepo = roleRepo;
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
                List<Claim> claimList = new List<Claim>();
                claimList.Add(new Claim("UserId", userEntity.Id));
                claimList.Add(new Claim(ClaimTypes.Role, roleEntity?.Id.ToString()));
                claimList.Add(new Claim(ClaimTypes.Role, roleEntity?.RoleName));
                return GenerateJwtToken(claimList);
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
                    AccountBalance = CommonConstants.DefaultCreditBalance
                });

                await transaction.SaveChangesAsync();

                var roleEntity = await _roleRepo.UnTrackableQuery().FirstOrDefaultAsync(x => x.Id == userEntity.RoleId);
                List<Claim> claimList = new List<Claim>();
                claimList.Add(new Claim("UserId", userEntity.Id));
                claimList.Add(new Claim(ClaimTypes.Role, roleEntity?.Id.ToString()));
                claimList.Add(new Claim(ClaimTypes.Role, roleEntity?.RoleName));

                return GenerateJwtToken(claimList);
            }

            //EmailAddress email = _emailIdRepo.AsQueryable().FirstOrDefault(x => Convert.ToString(x.EmailAddress1) == user.DefaultEmail);
            //if (email != null)
            //{
            //    token = CommonConstants.HttpResponseMessages.MailExists;
            //    return null;
            //}

            //_emailIdRepo.Add(new EmailAddress
            //{
            //    UserId = userId,
            //    IsPrimaryMail = CommonConstants.True,
            //    EmailAddress1 = user.DefaultEmail,
            //    Status = CommonConstants.StatusTypes.Pending
            //});
        }

        // All User API
        public Task<UserModel> UpdateOwnInformation(UserModel userModel) => throw new NotImplementedException();
        public Task<UserModel> GetOwnInformation(string userId) => throw new NotImplementedException();
        public Task<bool> ArchiveOwnId(string userId) => throw new NotImplementedException();

        // Organization Admin API
        public Task<UserModel> RegisterNewUser(UserModel userModel) => throw new NotImplementedException();
        public Task<bool> ArchiveAccount(string userId) => throw new NotImplementedException();
        public Task<bool> ResetPassword(string userId) => throw new NotImplementedException();

        // System Admin API
        public Task<bool> DeleteAccount(string userId) => throw new NotImplementedException();
        
        private string GenerateJwtToken(List<Claim> claimList)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] tokenKey = Encoding.ASCII.GetBytes(CommonConstants.PasswordConfig.Salt);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(CommonConstants.PasswordConfig.SaltExpire),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        //public bool ArchiveAccount(string userId)
        //{
        //    try
        //    {
        //        User user = _userRepo.AsQueryable().FirstOrDefault(x => x.UserId == userId);
        //        if (user != null)
        //        {
        //            user.Status = CommonConstants.StatusTypes.Archived;
        //            _userRepo.Update(user);
        //            return true;
        //        }

        //        return false;
        //    }
        //    catch(Exception ex)
        //    {
        //        int pk = _crashLogRepo.AsQueryable().Count() + 1;

        //        _crashLogRepo.Add(new Crashlog
        //        {
        //            CrashLogId = pk,
        //            ClassName = "UserService",
        //            MethodName = "ArchiveAccount",
        //            ErrorMessage = ex.Message,
        //            ErrorInner = (string.IsNullOrEmpty(ex.Message) || ex.Message == CommonConstants.MsgInInnerException ? ex.InnerException.Message : ex.Message),
        //            Data = userId,
        //            TimeStamp = DateTime.Now
        //        });
        //        return false;
        //    }
        //}

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
