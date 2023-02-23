using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace RedBook.Core.Security
{
    public class HttpContextClaimsPrincipalAccessor : IClaimsPrincipalAccessor
    {
        protected IHttpContextAccessor HttpContextAccessor { get; }

        public HttpContextClaimsPrincipalAccessor(IHttpContextAccessor accessor)
        {
            HttpContextAccessor = accessor;
        }

        public virtual ClaimsPrincipal? GetCurrentPrincipal() => HttpContextAccessor?.HttpContext?.User;
    }
}
