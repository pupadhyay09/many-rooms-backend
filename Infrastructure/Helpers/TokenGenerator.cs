using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ManyRoomStudio.Infrastructure.Helpers
{
    public class TokenGenerator
    {
        private static readonly SymmetricSecurityKey _signingKey;

        static TokenGenerator()
        {
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfig.Get("JwtSettings:SecretKey")));
        }

        public static string GenerateToken(string userId, string userName, string rolename)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, rolename),
            }),
                //Expires = DateTime.UtcNow.AddDays(1), // Set the token expiration time
                Expires = DateTime.UtcNow.AddDays(365),
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
