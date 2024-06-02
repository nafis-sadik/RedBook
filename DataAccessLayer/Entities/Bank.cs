using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data.Entities;

public partial class Bank
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BankId { get; set; }

    public string BankName { get; set; }

    public virtual ICollection<BankBranch> BankBranches { get; set; } = new List<BankBranch>();
}
