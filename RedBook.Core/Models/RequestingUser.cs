using Microsoft.IdentityModel.Tokens;
using RedBook.Core.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RedBook.Core.Models
{
    public class RequestingUser
    {
        internal ClaimsPrincipal _contextReference;
        internal RequestingUser(ClaimsPrincipal contextReference)
        {
            _contextReference = contextReference;
        }

        public string UserId
        {
            get
            {
                string? guid = _contextReference.Claims.FirstOrDefault(x => x.Type.Equals("UserId", StringComparison.InvariantCultureIgnoreCase))?.Value;
                if (!string.IsNullOrWhiteSpace(guid))
                    return guid;
                else
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);
            }

        }

        public int[] RoleIds
        {
            get
            {
                string? roleIds = _contextReference.Claims.FirstOrDefault(x => x.Type.Equals("UserRoleIds", StringComparison.InvariantCultureIgnoreCase))?.Value;
                if (!string.IsNullOrEmpty(roleIds))
                {
                    string[] strArray = roleIds.Split(',');
                    return Array.ConvertAll(strArray, int.Parse);
                }
                else
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);
            }
        }
    }
}
