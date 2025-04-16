using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO.Auth;
using WebApplication2.Services.Auth;

namespace WebApplication2.Controllers.Dashboard
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDTO>> Login(LoginDTO login)
        {
            var result = await _authService.Login(login);
            
            if (result == null)
                return Unauthorized("Неверный логин или пароль");

            return Ok(result);
        }
    }
} 