using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data.Entities;

public partial class BankAccount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AccountId { get; set; }

    public string AccountName { get; set; }

    [ForeignKey("BankBranch")]
    public int BranchId { get; set; }

    public int OrganizationId { get; set; }

    public virtual BankBranch Branch { get; set; }
}
