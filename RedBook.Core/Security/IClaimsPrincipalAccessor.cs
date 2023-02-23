using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RedBook.Core.Security
{
    public interface IClaimsPrincipalAccessor
    {
        ClaimsPrincipal GetCurrentPrincipal();
    }
}
