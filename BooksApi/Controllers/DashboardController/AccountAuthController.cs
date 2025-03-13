using Microsoft.AspNetCore.Mvc;
using BooksApi.DTO.Dashboard;
using BooksApi.Models.Dashboard;
using BooksApi.Service.DashboardService;

namespace BooksApi.Controllers.Dashboard
{
    [Route("api/user/auth")]
    [ApiController]
    public class AccountAuthService : ControllerBase
    {
        private readonly UserAuthForm _auth;

        public AccountAuthService(UserAuthForm user)
        {
            _auth = user;
        }

        [HttpPost]
        public async Task<IActionResult> SignInTask([FromBody] LoginDto userDto)
        {
            try
            {
                var account = new Account
                {
                    Login = userDto.Login,
                    Password = userDto.Password
                };
                var user = await _auth.SignTask(account);

                if (user == null)
                {
                    return Unauthorized(new { message = "Неверный логин или пароль" });
                }
                var acc = new AccountsDTO
                {
                    Id = user.Id,
                    Login = user.Login,
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Role = user.Role
                };
                return Ok(new { message = "Пользователь успешно авторизовался", user = acc });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка авторизации", error = ex.Message });
            }
        }
    }
}