namespace Identity.Data.Models
{
    public class RoleModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int OrganizationId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
