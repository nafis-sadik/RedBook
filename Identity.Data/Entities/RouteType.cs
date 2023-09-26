using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Identity.Data.Entities;
public partial class RouteType
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RouteTypeId { get; set; }
    [Required]
    public string RouteTypeName { get; set; }
    public virtual ICollection<Route> Routes { get; set; } = new List<Route>();
}