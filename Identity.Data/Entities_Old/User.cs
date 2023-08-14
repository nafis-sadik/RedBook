using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class User
{
    public string UserId { get; set; }

    public string UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }

    public int OrganizationId { get; set; }

    public int RoleId { get; set; }

    public string Status { get; set; }

    public int? AccountBalance { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual Role Role { get; set; }
}
