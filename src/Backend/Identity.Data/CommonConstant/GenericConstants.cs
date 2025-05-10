using Identity.Data.Entities;

namespace Identity.Data.CommonConstant
{
    public static class GenericConstants
    {
        public static Application RedBookFrontEnd = new Application
        {
            ApplicationName = "Redbook Angular",
            ApplicationUrl = "http://localhost:4200",
        };

        public static Application BlumeIdentity = new Application
        {
            ApplicationName = "Blume Identity",
            ApplicationUrl = "http://localhost:5062"
        };

        public static Application RedbookAPI = new Application
        {
            ApplicationName = "Redbook API",
            ApplicationUrl = "http://localhost:7238"
        };
    }
}
