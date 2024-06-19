using Identity.Data.Entities;

namespace Identity.Data.CommonConstant
{
    public class RoleConstants
    {
        public static Role SystemAdmin = new Role
        {
            RoleName = "System Admin",
            IsAdmin = true,
            IsRetailer = true,
            IsSystemAdmin = true,
            IsOwner = true,
            OrganizationId = null,
            ApplicationId = 1,
        };

        public static Role RedbookAdmin = new Role
        {
            RoleName = "Redbook Admin",
            IsAdmin = true,
            IsRetailer = false,
            IsSystemAdmin = false,
            IsOwner = false,
            OrganizationId = null,
            ApplicationId = 1,
        };

        public static Role OwnerAdmin = new Role
        {
            RoleName = "Redbook Owner Admin",
            IsAdmin = true,
            IsRetailer = false,
            IsSystemAdmin = false,
            IsOwner = true,
            OrganizationId = null,
            ApplicationId = 1,
        };

        public static Role Retailer = new Role
        {
            RoleName = "Retailer",
            IsAdmin = false,
            IsRetailer = true,
            IsSystemAdmin = false,
            IsOwner = false,
            OrganizationId = null,
            ApplicationId = 1,
        };
    }
}
