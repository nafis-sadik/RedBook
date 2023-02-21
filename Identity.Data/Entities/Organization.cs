#nullable disable

namespace Identity.Data.Entities
{
    public partial class Organization
    {
        public Organization()
        {
            Roles = new HashSet<Role>();
            UserGroups = new HashSet<UserGroup>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string OrganizationName { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
