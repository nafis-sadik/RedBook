using Identity.Data.Entities;

namespace Identity.Data
{
    public static class RouteTypeConsts
    {
        public static RouteType GenericRoute = new RouteType { RouteTypeId = 1, RouteTypeName = "Generic Route" };
        public static RouteType AdminRoute = new RouteType { RouteTypeId = 2, RouteTypeName = "Admin Route" };
        public static RouteType RetailerRoute = new RouteType { RouteTypeId = 3, RouteTypeName = "Retailer Route" };
        public static RouteType SysAdminRoute = new RouteType { RouteTypeId = 4, RouteTypeName = "System Admin Route" };
    }
}
