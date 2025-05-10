namespace Identity.Data.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicLoc { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public RoleModel[] UserRoles { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }
        public decimal? AccountBalance { get; set; }
        public string Email { get; set; }
        //public int OrganizationId { get; set; }
        public int ApplicationId { get; set; }
        public bool RememberMe { get; set; }
    }
}
