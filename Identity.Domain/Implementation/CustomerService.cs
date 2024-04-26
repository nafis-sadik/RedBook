using Identity.Data.CommonConstant;
using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Domain;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;

namespace Identity.Domain.Implementation
{
    public class CustomerService: ServiceBase, ICustomerService
    {
        private IRepositoryFactory _repositoryFactory;
        public CustomerService(
            ILogger<CustomerService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor)
        { }

        public async Task OnboardUser(UserModel userModel, OrganizationModel org)
        {
            using(_repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var orgRepo = _repositoryFactory.GetRepository<Organization>();
                var userRepo = _repositoryFactory.GetRepository<User>();
                var roleRepo = _repositoryFactory.GetRepository<Role>();
                var userRoleRepo = _repositoryFactory.GetRepository<UserRoleMapping>();

                if (string.IsNullOrEmpty(userModel.Email))
                    throw new ArgumentException("Email not provided");

                User? newUser = await userRepo.TrackableQuery().FirstOrDefaultAsync(x => userModel.Email.Equals(x.Email));
                if (newUser == null)
                {
                    newUser = Mapper.Map<User>(userModel);
                    newUser.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(CommonConstants.PasswordConfig.DefaultPassword);
                    newUser.Status = true;
                    newUser = await userRepo.InsertAsync(newUser);
                    await _repositoryFactory.SaveChangesAsync();
                }

                // Creating the new organization
                Organization orgEntity = Mapper.Map<Organization>(org);
                orgEntity.CreatedBy = User.UserId;
                orgEntity.CreateDate = DateTime.UtcNow;
                orgEntity = await orgRepo.InsertAsync(orgEntity);
                await _repositoryFactory.SaveChangesAsync();

                // Creating admin role for the organization
                Role userRole = RoleConstants.OwnerAdmin;
                userRole.OrganizationId = orgEntity.OrganizationId;
                Role? adminRoleForNewOrg = await roleRepo.InsertAsync(userRole);
                await _repositoryFactory.SaveChangesAsync();

                await userRoleRepo.InsertAsync(new UserRoleMapping
                {
                    RoleId = userRole.RoleId,
                    UserId = newUser.UserId,
                });
                await _repositoryFactory.SaveChangesAsync();
            }
        }
    }
}
