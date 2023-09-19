using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Data.Entities
{
    public partial class UserRole
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int RoleId { get; set; }

        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}
