using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Data.Entities;

public partial class RouteType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RouteTypeId { get; set; }

    public string RouteTypeName { get; set; }

    public virtual ICollection<Route> Routes { get; set; } = new List<Route>();
}
