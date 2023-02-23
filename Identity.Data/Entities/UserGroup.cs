namespace Identity.Data.Entities
{
    public partial class UserGroup
    {
        public UserGroup()
        {
            Policies = new HashSet<Policy>();
        }

        public int Id { get; set; }
        public string UserGroupName { get; set; }
        public int OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual ICollection<Policy> Policies { get; set; }
    }
}
