using Identity.Data.Entities;
using Microsoft.EntityFrameworkCore;
using RedBook.Core.Domain;
using RedBook.Core.Repositories;

namespace Identity.Domain
{
    internal static class HelperClass
    {
        internal static async Task<bool> HasSystemAdminPriviledge(this ServiceBase serviceBase, IRepositoryBase<Role> roleRepo)
            => await roleRepo.UnTrackableQuery().Where(x => serviceBase.User.RoleIds.Contains(x.RoleId) && x.IsSystemAdmin).CountAsync() > 0;
        

        internal static async Task<bool> HasAdminPriviledge(this ServiceBase serviceBase, IRepositoryBase<Role> roleRepo, int orgId)
            => await roleRepo.UnTrackableQuery().Where(x => serviceBase.User.RoleIds.Contains(x.RoleId) && x.OrganizationId == orgId && x.IsAdmin).CountAsync() > 0;


        internal static async Task<bool> HasAdminPriviledge(this ServiceBase serviceBase, IRepositoryBase<Role> roleRepo, int[] orgId)
            => await roleRepo.UnTrackableQuery().Where(x => serviceBase.User.RoleIds.Contains(x.RoleId) && orgId.Contains(x.OrganizationId) && x.IsAdmin).CountAsync() > 0;

        internal static async Task<bool> HasRetailerPriviledge(this ServiceBase serviceBase, IRepositoryBase<Role> roleRepo)
            => await roleRepo.UnTrackableQuery().Where(x => serviceBase.User.RoleIds.Contains(x.RoleId) && x.IsRetailer).CountAsync() > 0;
    }
}
