using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Data.Models
{
    public class PolicyModel
    {
        public int Id { get; set; }

        public int OrganizationId { get; set; }

        public int RouteId { get; set; }

        public int RoleId { get; set; }

        public int UserGroupId { get; set; }

        public bool Authorize { get; set; }

    }
}
