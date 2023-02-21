#nullable disable

namespace Identity.Data.Entities
{
    public partial class Role
    {
        public Role()
        {
            Policies = new HashSet<Policy>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }
        public int OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual ICollection<Policy> Policies { get; set; }
    }
}
