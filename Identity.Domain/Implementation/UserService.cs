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
            User? userEntity;
            Role? roleEntity;
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                _userRepo = unitOfWork.GetRepository<User>();
                _roleRepo = unitOfWork.GetRepository<Role>();
                userEntity = await _userRepo
                    .UnTrackableQuery()
                    .FirstOrDefaultAsync(x => 
                        x.UserName == userModel.UserName 
                        || x.UserId == userModel.UserId 
                        || x.Email == userModel.Email
                    );
                if(userEntity == null) throw new ArgumentException("User not found");
                roleEntity = await _roleRepo
                    .UnTrackableQuery()
                    .FirstOrDefaultAsync(x => x.RoleId == userEntity.RoleId);
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

        // User API
        public async Task<UserModel> UpdateAsync(UserModel userModel)
        {
            string? requestSenderId = User?.Claims.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value;
            if (userModel.UserId != requestSenderId)
                throw new ArgumentException($"User {requestSenderId} is not allowed to update information of {userModel.UserId}");

            string? requesterRoleIdStr = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;
            if (string.IsNullOrEmpty(requesterRoleIdStr) || !int.TryParse(requesterRoleIdStr, out int requesterRoleId))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            User? userEntity;

            using (var transaction = UnitOfWorkManager.Begin())
            {
                _userRepo = transaction.GetRepository<User>();
                userEntity = await _userRepo.GetByIdAsync(userModel.UserId);

                if (userEntity == null)
                    throw new ArgumentException($"User with identifier {userModel.UserId} was not found");

                // Update own data
                if (userModel.UserId == requestSenderId)
                {
                    Mapper.Map(userEntity, userModel);
                }
                // Update employe data
                else
                {
                    userEntity.FirstName = userModel.FirstName;
                    userEntity.LastName = userModel.LastName;
                    userEntity.UserName = userModel.UserName;
                    userEntity.OrganizationId = userModel.OrganizationId;
                }
                
                userEntity = _userRepo.Update(userEntity);

                await transaction.SaveChangesAsync();
            }

            return Mapper.Map<UserModel>(userEntity);
        }

        public async Task<UserModel?> GetById(string userId)
        {
            string? requestingUserId = User?.Claims.FirstOrDefault(x => x.Type.Equals("UserId", StringComparison.InvariantCultureIgnoreCase))?.Value;
            if(string.IsNullOrEmpty(requestingUserId))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);


            string? requesterRoleIdStr = User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;
            if (string.IsNullOrEmpty(requesterRoleIdStr) || !int.TryParse(requesterRoleIdStr, out int requesterRoleId))
                throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

            User? userEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                _userRepo = transaction.GetRepository<User>();

                // Requesting own details
                if(requestingUserId == userId)
                    userEntity = await _userRepo.GetByIdAsync(userId);
                // Admin requesting employee details
                else
                {
                    _roleRepo = transaction.GetRepository<Role>();
                    Role? requestingUserRoleEntity = await _roleRepo.GetByIdAsync(requesterRoleId);

                    // Role existance check
                    if(requestingUserRoleEntity == null)
                        throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);

                    // Admin priviledge check
                    if (requestingUserRoleEntity.IsAdminRole != 1)
                        throw new ArgumentException(CommonConstants.HttpResponseMessages.AdminAccessRequired);

                    userEntity = await _userRepo.GetByIdAsync(userId);
                }

                if(userEntity == null)
                    throw new ArgumentException($"User with identifier {userId} was not found");
            }

            return Mapper.Map<UserModel>(userEntity);
        }

        public async Task<bool> ArchiveAccount(string userId)
        {
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

        public async Task ResetPassword(string userId)
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
                var _orgRepo = transaction.GetRepository<Organization>();

                User? userEntity = await _userRepo.GetByIdAsync(requesterRoleIdStr);
                if (userEntity == null)
                    throw new ArgumentException($"User with identifier {userId} was not found");

                Organization? orgEntity = await _orgRepo.GetByIdAsync(userEntity.OrganizationId);
                if (orgEntity == null)
                    throw new ArgumentException($"User with identifier {userId} was not found");

                if(!orgEntity.OrganizationName.Trim().ToLower().Contains("blumedigital"))
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                // Role Id for System Admin should always be 1
                // Only System Admin user can unarchive an user
                var requesterRoleEntity = await _roleRepo.GetByIdAsync(requesterRoleId);
                if (requesterRoleEntity == null || requesterRoleEntity.IsAdminRole != 1)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                userEntity = await _userRepo.GetByIdAsync(userId);
                if (userEntity == null)
                    throw new ArgumentException($"User with identifier {userId} was not found");

                userEntity.Password = BCrypt.Net.BCrypt.EnhancedHashPassword("12345678");
                _userRepo.Update(userEntity);

                await transaction.SaveChangesAsync();
            }
        }

        // Admin Only API
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

                User? userEntity = await _userRepo.GetByIdAsync(userId);
                if (userEntity == null)
                    throw new ArgumentException($"User with identifier {userId} was not found");

                // Role Id for System Admin should always be 1
                // Only System Admin user can unarchive an user
                var requesterRoleEntity = await _roleRepo.GetByIdAsync(requesterRoleId);
                if (requesterRoleEntity == null || requesterRoleEntity.IsAdminRole != 1)
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

                User? userEntity = await _userRepo.GetByIdAsync(userId);
                if (userEntity == null)
                    throw new ArgumentException($"User with identifier {userId} was not found");

                // Role Id for System Admin should always be 1
                // Only System Admin user can unarchive an user
                var requesterRoleEntity = await _roleRepo.GetByIdAsync(requesterRoleId);
                if (requesterRoleEntity == null || requesterRoleEntity.IsAdminRole != 1)
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.NotAllowed);

                userEntity.Status = CommonConstants.StatusTypes.Active.ToString();
                _userRepo.Update(userEntity);
                await transaction.SaveChangesAsync();
            }
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
