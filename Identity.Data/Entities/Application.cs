#nullable disable

namespace Identity.Data.Entities
{
    public partial class Application
    {
        public Application()
        {
            Routes = new HashSet<Route>();
        }

        public int Id { get; set; }
        public string ApplicationName { get; set; }

        public virtual ICollection<Route> Routes { get; set; }
    }
}
