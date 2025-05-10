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
    public class OnboardingService : ServiceBase, IOnboardingService
    {
        public OnboardingService(
            ILogger<OnboardingService> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork, httpContextAccessor) { }

        public async Task RedbookOnboardingAsync(OnboardingModel model)
        {
            using (var _repositoryFactory = UnitOfWorkManager.GetRepositoryFactory())
            {
                var orgRepo = _repositoryFactory.GetRepository<Organization>();
                var userRepo = _repositoryFactory.GetRepository<User>();
                var roleRepo = _repositoryFactory.GetRepository<Role>();
                var userRoleRepo = _repositoryFactory.GetRepository<UserRoleMapping>();

                if (string.IsNullOrEmpty(model.User.Email))
                    throw new ArgumentException("Email not provided");

                User newUser = await userRepo.TrackableQuery().FirstOrDefaultAsync(x => model.User.Email.Equals(x.Email));
                if (newUser == null)
                {
                    newUser = Mapper.Map<User>(model.User);
                    newUser.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(CommonConstants.PasswordConfig.DefaultPassword);
                    newUser.Status = true;
                    newUser = await userRepo.InsertAsync(newUser);
                    await _repositoryFactory.SaveChangesAsync();
                }

                // Creating the new organization
                Organization orgEntity = Mapper.Map<Organization>(model.Organization);
                orgEntity.CreatedBy = User.UserId;
                orgEntity.CreateDate = DateTime.UtcNow;
                orgEntity = await orgRepo.InsertAsync(orgEntity);
                await _repositoryFactory.SaveChangesAsync();

                // Assign the role to the user by mapping
                await userRoleRepo.InsertAsync(new UserRoleMapping
                {
                    RoleId = RoleConstants.Owner.RoleId,
                    UserId = newUser.UserId,
                    OrganizationId = orgEntity.OrganizationId,
                });
                await _repositoryFactory.SaveChangesAsync();
            }
        }
    }
}
