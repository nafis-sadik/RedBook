using Identity.Data.Entities;

namespace Identity.Data.CommonConstant
{
    public class RoleConstants
    {
        public static Role SystemAdmin = new Role
        {
            RoleName = "Redbook - System Admin",
            IsAdmin = true,
            IsRetailer = true,
            IsSystemAdmin = true,
            IsOwner = true,
            OrganizationId = null,
            ApplicationId = 1,
        };

        public static Role Owner = new Role
        {
            RoleName = "Organization Owner",
            IsAdmin = true,
            IsRetailer = false,
            IsSystemAdmin = false,
            IsOwner = false,
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
