using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BooksApi.Models.Dashboard;

namespace BooksApi.Service
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _tokenExpirationDays;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _secretKey = _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");
            _issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured");
            _audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured");
            _tokenExpirationDays = _configuration.GetValue<int>("Jwt:TokenExpirationDays", 7);
        }

        public string GenerateToken(Account user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_tokenExpirationDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secretKey);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token cannot be null or empty", nameof(token));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            return principal;
        }

        public int GetUserIdFromToken(string token)
        {
            var principal = GetClaimsPrincipal(token);
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new InvalidOperationException("Invalid token: User ID not found");
            }

            return userId;
        }

        public string GetUserRoleFromToken(string token)
        {
            var principal = GetClaimsPrincipal(token);
            var roleClaim = principal.FindFirst(ClaimTypes.Role);
            
            if (roleClaim == null)
            {
                throw new InvalidOperationException("Invalid token: Role not found");
            }

            return roleClaim.Value;
        }

        public bool IsTokenExpired(string token)
        {
            var principal = GetClaimsPrincipal(token);
            var expirationClaim = principal.FindFirst(JwtRegisteredClaimNames.Exp);
            
            if (expirationClaim == null || !long.TryParse(expirationClaim.Value, out long expiration))
            {
                return true;
            }

            var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(expiration).UtcDateTime;
            return DateTime.UtcNow >= expirationDateTime;
        }
    }
} 