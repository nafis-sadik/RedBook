namespace Identity.Data.Entities
{
    public partial class Policy
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int RouteId { get; set; }
        public int RoleId { get; set; }
        public int UserGroupId { get; set; }
        public bool Authorize { get; set; }

        public virtual Role Role { get; set; }
        public virtual Route Route { get; set; }
        public virtual UserGroup UserGroup { get; set; }
    }
}
