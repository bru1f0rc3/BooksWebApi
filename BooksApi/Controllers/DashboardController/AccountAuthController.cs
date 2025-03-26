using Microsoft.AspNetCore.Mvc;
using BooksApi.DTO.Dashboard;
using BooksApi.Models.Dashboard;
using BooksApi.Service.DashboardService;
using BooksApi.Service;

namespace BooksApi.Controllers.Dashboard
{
    [Route("api/user/auth")]
    [ApiController]
    public class AccountAuthService : ControllerBase
    {
        private readonly UserAuthForm _auth;
        private readonly JwtService _jwtService;

        public AccountAuthService(UserAuthForm user, JwtService jwtService)
        {
            _auth = user;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignInTask([FromBody] LoginDto loginDto)
        {
            try
            {
                var account = new Account
                {
                    Login = loginDto.Login,
                    Password = loginDto.Password
                };
                var user = await _auth.SignTask(account);

                if (user == null)
                {
                    return Unauthorized(new { message = "Неверный логин или пароль" });
                }

                var token = _jwtService.GenerateToken(user);
                var acc = new AccountsDTO
                {
                    Id = user.Id,
                    Login = user.Login,
                    FullName = user.FullName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Phone = user.Phone ?? string.Empty,
                    Role = user.Role
                };

                return Ok(new AuthResponseDto
                {
                    Token = token,
                    User = acc,
                    Message = "Пользователь успешно авторизовался"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка авторизации", error = ex.Message });
            }
        }

        [HttpGet("validate")]
        public IActionResult ValidateToken([FromHeader(Name = "Authorization")] string token)
        {
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return BadRequest(new { message = "Токен не предоставлен" });
            }

            token = token.Substring("Bearer ".Length);
            var isValid = _jwtService.ValidateToken(token);

            if (!isValid)
            {
                return Unauthorized(new { message = "Недействительный токен" });
            }

            return Ok(new { message = "Токен действителен" });
        }
    }
}