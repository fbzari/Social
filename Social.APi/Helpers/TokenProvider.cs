using Microsoft.IdentityModel.Tokens;
using Social.APi.Models;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Social.APi.Helpers
{
    internal sealed class TokenProvider(IConfiguration configuration)
    {

        public string create(User user)
        {
            string secretkey = configuration["Jwt:Secrets"];
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));

            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var roles = user.UserRoles?.Select(ur => ur.Role?.Name).Where(name => name != null).ToList();

            if (roles == null || roles.Count == 0)
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }
            else
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }


            var tokendescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpiryInMinutes")),
                SigningCredentials = credentials,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokendescriptor);

            return token;
        }


    }
}
