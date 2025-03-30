using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApplication2.Connection;
using WebApplication2.DTO.Auth;

namespace WebApplication2.Services.Auth
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<TokenDTO?> Login(LoginDTO login)
        {
            const string sql = @"
                SELECT a.id, a.login, a.password, a.full_name, r.name as role_name
                FROM ""Accounts"" a
                JOIN ""Roles"" r ON r.id = a.role_id
                WHERE a.login = @Login AND a.password = @Password";

            var user = await DbConnect.QueryFirstOrDefaultAsync<dynamic>(sql, login);

            if (user == null)
                return null;

            var token = GenerateJwtToken(user);

            return new TokenDTO
            {
                Token = token,
                Role = user.role_name,
                UserId = user.id,
                FullName = user.full_name
            };
        }

        private string GenerateJwtToken(dynamic user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.full_name),
                new Claim(ClaimTypes.Role, user.role_name)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 