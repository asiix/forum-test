using Microsoft.IdentityModel.Tokens;
using ForumUsers.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ForumUsers.Authentication
{
    public class JwtManager
    {
        // Creazione del token
        public static string GenerateJwtToken(User user, IConfiguration configuration)
        {
            var secretKey = configuration["Jwt:Key"];
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Cosa inserire nel token
            var claims = new ClaimsIdentity(new Claim[] {
                new Claim("Role", user.Role),
            });

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims.Claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            return tokenHandler.WriteToken(token);
        }
    }
}
