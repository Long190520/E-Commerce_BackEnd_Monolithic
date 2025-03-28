using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce_BackEnd.Utils
{
    public static class UserUtils
    {
        public static ClaimsPrincipal GetPrincipalFromAccessToken(string accessToken, IConfiguration _configuration)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            ClaimsPrincipal result = jwtSecurityTokenHandler.ValidateToken(accessToken, validationParameters, out validatedToken);
            if (!(validatedToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals("http://www.w3.org/2001/04/xmldsig-more#hmac-sha256", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return result;
        }
    }
}
