using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationSystemAPI.Services.Interfaces
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _opts;

        public JwtService(IOptions<JwtOptions> opts)
        {
            _opts = opts.Value;
        }

        public string CreateToken(ApplicationUser user)
        {
            var claims = new[]
            {
               new Claim("Id", user.Id.ToString()),
               new Claim("Email", user.Email),
               new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _opts.Issuer,
                audience: _opts.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class JwtOptions
    {
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public string Key { get; set; } = "";
    }
}
