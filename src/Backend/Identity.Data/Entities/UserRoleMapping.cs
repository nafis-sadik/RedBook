using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Data.Entities;

public partial class UserRoleMapping
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserRoleId { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    public int OrganizationId { get; set; }

    public DateTime CreateDate { get; set; }
    
    public int CreateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? UpdateBy { get; set; }

    public bool Status { get; set; }

    [ForeignKey("RoleId")]
    public virtual Role Role { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    [ForeignKey("OrganizationId")]
    public virtual Organization Organization { get; set; }
}
