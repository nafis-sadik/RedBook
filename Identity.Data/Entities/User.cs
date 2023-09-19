namespace Identity.Data.Entities;

public partial class User
{
    public string UserId { get; set; }

    public string UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }

    public string Status { get; set; }

    public string Email { get; set; }

    public int? AccountBalance { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
