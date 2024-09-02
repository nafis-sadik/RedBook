namespace Identity.Data.Models
{
    public class RouteModel
    {
        public int RouteId { get; set; }

        public string RouteName { get; set; }

        public string RouteValue { get; set; }

        public string Description { get; set; }

        public int ApplicationId { get; set; }

        public int Sequence { get; set; }

        public int? ParentRouteId { get; set; }

        public int RouteTypeId { get; set; }

        public bool IsMenuRoute { get; set; }

        public string ApplicationName { get; set; }
    }
}
