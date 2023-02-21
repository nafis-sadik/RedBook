#nullable disable

namespace Identity.Data.Entities
{
    public partial class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public int? OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
