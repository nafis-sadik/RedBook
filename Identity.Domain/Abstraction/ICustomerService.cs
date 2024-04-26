using Identity.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Abstraction
{
    public interface ICustomerService
    {
        public Task OnboardUser(UserModel user, OrganizationModel org);
    }
}
