namespace Identity.Data.Entities;

public partial class Organization
{
    public int Id { get; set; }

    public string OrganizationName { get; set; }

    public virtual ICollection<OrganizationRoleMapping> OrganizationRoleMappings { get; } = new List<OrganizationRoleMapping>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
