using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace user.Services
{
    public class JwtService
    {
        private readonly string _jwtKey;

        public JwtService()
        {
            _jwtKey = Environment.GetEnvironmentVariable("USER_API_Jwt_key")
                ?? throw new Exception("JWT key missing");
        }

        public string GenerateJwt(
            string email,
            string role,
            Guid personGuid)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim("personGuid", personGuid.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtKey));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}