using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Social.APi.Extensions
{
    public class Helper
    {
        public static string GetSenderEmailFromJwt(HttpContext httpContext)
        {
            var user = httpContext.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                var emailClaim = user.FindFirst(ClaimTypes.Email);
                return emailClaim?.Value;
            }
            return null;
        }

        public static string GetSenderIdFromJwt(HttpContext httpContext)
        {
            var user = httpContext.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                var emailClaim = user.FindFirst(ClaimTypes.Name);
                return emailClaim?.Value;
            }
            return null;
        }
    }
}
