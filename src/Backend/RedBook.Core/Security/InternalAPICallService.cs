using Microsoft.IdentityModel.Tokens;
using RedBook.Core.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RedBook.Core.Security
{
    public class InternalAPICallService
    {
        public static string JWTForInternal
        {
            get
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                byte[] tokenKey = Encoding.ASCII.GetBytes(CommonConstants.PasswordConfig.Salt);
                SecurityToken token = new JwtSecurityToken
                (
                    issuer: "Blume.Inventory",
                    audience: "Client",
                    expires: DateTime.UtcNow.AddDays(CommonConstants.PasswordConfig.SaltExpire),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature
                    ),
                    claims: new List<Claim> {
                        new Claim("UserId", "Internal User"),
                        new Claim("UserName", "Internal User")
                    }
                );

                return tokenHandler.WriteToken(token);
            }

            private set { }
        }
    }
}
