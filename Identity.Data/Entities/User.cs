using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }

    public bool Status { get; set; }

    public int AccountBalance { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Address { get; set; }

    public virtual ICollection<SubscriptionTransaction> SubscriptionTransactions { get; set; } = new List<SubscriptionTransaction>();

    public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; } = new List<UserRoleMapping>();
}
