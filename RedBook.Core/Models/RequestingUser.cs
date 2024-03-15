using RedBook.Core.Constants;
using System.Security.Claims;

namespace RedBook.Core.Models
{
    public class RequestingUser
    {
        internal ClaimsPrincipal _contextReference;
        internal RequestingUser(ClaimsPrincipal contextReference)
        {
            _contextReference = contextReference;
        }

        public int UserId
        {
            get
            {
                string? _userId = _contextReference.Claims.FirstOrDefault(x => x.Type.Equals("UserId", StringComparison.InvariantCultureIgnoreCase))?.Value;
                if (!string.IsNullOrWhiteSpace(_userId) && int.TryParse(_userId, out int userId))
                    return userId;
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
