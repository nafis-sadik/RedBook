using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace RedBook.Core.Security
{
    public class HttpContextClaimsPrincipalAccessor : IClaimsPrincipalAccessor
    {
        private IHttpContextAccessor HttpContextAccessor { get; }

        public HttpContextClaimsPrincipalAccessor(IHttpContextAccessor accessor)
        {
            HttpContextAccessor = accessor;
        }

        public virtual ClaimsPrincipal GetCurrentPrincipal()
        {
            if(HttpContextAccessor == null)
                throw new ArgumentException("Error in HttpContextAccessor");

            if(HttpContextAccessor.HttpContext == null)
                throw new ArgumentException("Error in HttpContextAccessor.HttpContext");
            
            return HttpContextAccessor.HttpContext.User;

        }
    }
}
