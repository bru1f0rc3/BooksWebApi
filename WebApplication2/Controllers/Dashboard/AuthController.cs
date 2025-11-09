using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO.Auth;
using WebApplication2.Interfaces;

namespace WebApplication2.Controllers.Dashboard
{
    /// <summary>
    /// Контроллер для аутентификации пользователей
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Конструктор контроллера аутентификации
        /// </summary>
        /// <param name="authService">Сервис аутентификации</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Вход пользователя в систему
        /// </summary>
        /// <param name="login">Данные для входа (логин и пароль)</param>
        /// <returns>JWT токен и информация о пользователе при успешной аутентификации</returns>
        /// <response code="200">Успешная аутентификация</response>
        /// <response code="401">Неверный логин или пароль</response>
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