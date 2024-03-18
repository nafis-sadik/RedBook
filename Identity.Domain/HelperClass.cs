using Identity.Data.Entities;
using Microsoft.EntityFrameworkCore;
using RedBook.Core.Domain;
using RedBook.Core.Repositories;

namespace Identity.Domain
{
    internal static class HelperClass
    {
        internal static async Task<bool> HasSystemAdminPriviledge(this ServiceBase serviceBase, IRepositoryBase<UserRoleMapping> userRoleMappingRepo)
            => await userRoleMappingRepo.UnTrackableQuery().AnyAsync(m => m.UserId == serviceBase.User.UserId && m.Role.IsSystemAdmin == true);

        internal static async Task<bool> HasAdminPriviledge(this ServiceBase serviceBase, IRepositoryBase<UserRoleMapping> userRoleMappingRepo, int orgId)
            => await userRoleMappingRepo.UnTrackableQuery().AnyAsync(m => m.UserId == serviceBase.User.UserId && m.Role.OrganizationId == orgId && m.Role.IsAdmin == true);

        internal static async Task<bool> HasOwnerAdminPriviledge(this ServiceBase serviceBase, IRepositoryBase<UserRoleMapping> userRoleMappingRepo, int orgId)
            => await userRoleMappingRepo.UnTrackableQuery().AnyAsync(m => m.UserId == serviceBase.User.UserId && m.Role.OrganizationId == orgId && m.Role.IsOwner);

        internal static async Task<bool> HasRetailerPriviledge(this ServiceBase serviceBase, IRepositoryBase<UserRoleMapping> userRoleMappingRepo)
            => await userRoleMappingRepo.UnTrackableQuery().AnyAsync(m => m.UserId == serviceBase.User.UserId && m.Role.IsRetailer == true);
    }
}
