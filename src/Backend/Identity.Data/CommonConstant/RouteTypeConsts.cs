using Identity.Data.Entities;

namespace Identity.Data.CommonConstant
{
    public static class RouteTypeConsts
    {
        public static RouteType GenericRoute = new RouteType { RouteTypeName = "Generic Route" };
        public static RouteType AdminRoute = new RouteType { RouteTypeName = "Admin Route" };
        public static RouteType RetailerRoute = new RouteType { RouteTypeName = "Retailer Route" };
        public static RouteType SysAdminRoute = new RouteType { RouteTypeName = "System Admin Route" };
    }
}
