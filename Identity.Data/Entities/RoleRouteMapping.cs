using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Data.Entities;

public partial class RoleRouteMapping
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MappingId { get; set; }

    public int RouteId { get; set; }

    public int RoleId { get; set; }

    public bool HasCreateAccess { get; set; }

    public bool HasUpdateAccess { get; set; }

    public bool HasDeleteAccess { get; set; }

    [ForeignKey("RoleId")]
    public virtual Role Role { get; set; }

    [ForeignKey("RouteId")]
    public virtual Route Route { get; set; }
}
