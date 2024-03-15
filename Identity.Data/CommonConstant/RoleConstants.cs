using Identity.Data.Entities;

namespace Identity.Data.CommonConstant
{
    public class RoleConstants
    {        
        public static Role SystemAdmin
        {
            private set { }
            get
            {
                return new Role
                {
                    RoleName = "System Admin",
                    IsAdmin = true,
                    IsRetailer = true,
                    IsSystemAdmin = true,
                    IsOwner = true
                };
            }
        }
        
        public static Role Admin
        {
            private set { }
            get
            {
                return new Role
                {
                    RoleName = "Admin",
                    IsAdmin = true,
                    IsRetailer = false,
                    IsSystemAdmin = false,
                    IsOwner = false
                };
            }
        }

        public static Role OwnerAdmin
        {
            private set { }
            get
            {
                return new Role
                {
                    RoleName = "Owner Admin",
                    IsAdmin = true,
                    IsRetailer = false,
                    IsSystemAdmin = false,
                    IsOwner = true
                };
            }
        }

        public static Role Employee
        {
            private set { }
            get
            {
                return new Role
                {
                    RoleName = "Employee",
                    IsAdmin = false,
                    IsRetailer = false,
                    IsSystemAdmin = false,
                    IsOwner = false
                };
            }
        }

        public static Role Retailer
        {
            private set { }
            get
            {
                return new Role
                {
                    RoleName = "Retailer",
                    IsAdmin = false,
                    IsRetailer = true,
                    IsSystemAdmin = false,
                    IsOwner = false
                };
            }
        }
    }
}
