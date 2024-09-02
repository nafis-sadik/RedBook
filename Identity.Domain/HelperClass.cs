using Identity.Data.Entities;
using Microsoft.EntityFrameworkCore;
using RedBook.Core.Domain;
using RedBook.Core.Repositories;

namespace Identity.Domain
{
    internal static class HelperClass
    {
        internal static async Task<bool> HasSystemAdminPriviledge(this ServiceBase serviceBase, IRepositoryBase<UserRoleMapping> userRoleMappingRepo)
            => await userRoleMappingRepo.UnTrackableQuery().AnyAsync(m => m.UserId == serviceBase.User.UserId && m.Role.IsSystemAdmin);

        internal static async Task<bool> HasApplicationAdminPriviledge(this ServiceBase serviceBase, IRepositoryBase<UserRoleMapping> userRoleMappingRepo, int appId)
            => await userRoleMappingRepo.UnTrackableQuery()
                .AnyAsync(userRoleMapping => userRoleMapping.UserId == serviceBase.User.UserId
                    && userRoleMapping.Role.IsAdmin);

        internal static async Task<bool> HasOwnerPriviledge(this ServiceBase serviceBase, IRepositoryBase<UserRoleMapping> userRoleMappingRepo)
            => await userRoleMappingRepo.UnTrackableQuery()
                .AnyAsync(userRoleMapping => userRoleMapping.UserId == serviceBase.User.UserId && userRoleMapping.Role.IsOwner);

        internal static async Task<bool> HasOrgAdminPriviledge(this ServiceBase serviceBase, IRepositoryBase<UserRoleMapping> userRoleMappingRepo, int orgId)
            => await userRoleMappingRepo.UnTrackableQuery().AnyAsync(m => m.UserId == serviceBase.User.UserId && m.OrganizationId == orgId && m.Role.IsAdmin);

        internal static async Task<bool> IsOwnerOf(this ServiceBase serviceBase, IRepositoryBase<UserRoleMapping> userRoleMappingRepo, int orgId)
            => await userRoleMappingRepo.UnTrackableQuery()
                .AnyAsync(userRoleMapping => userRoleMapping.UserId == serviceBase.User.UserId
                    && userRoleMapping.OrganizationId == orgId
                    && userRoleMapping.Role.IsOwner);

        internal static async Task<bool> HasRetailerPriviledge(this ServiceBase serviceBase, IRepositoryBase<UserRoleMapping> userRoleMappingRepo)
            => await userRoleMappingRepo.UnTrackableQuery().AnyAsync(m => m.UserId == serviceBase.User.UserId && m.Role.IsRetailer);
    }
}
