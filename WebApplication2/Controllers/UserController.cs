using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO.Dashboard;
using WebApplication2.Services.Dashboard;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("add-user")]
        public async Task<ActionResult> CreateUser(CreateAccountDTO user)
        {
            await _userService.CreateUserAsync(user);
            return Ok();
        }

        [HttpPut("change-email")]
        public async Task<ActionResult> ChangeEmail(ChangeEmailUserDTO changeEmail)
        {
            await _userService.UpdateEmailUser(changeEmail);
            return Ok();
        }

        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDTO changePassword)
        {
            if (string.IsNullOrEmpty(changePassword.old_password) || 
                string.IsNullOrEmpty(changePassword.new_password))
            {
                return BadRequest("Старый и новый пароли обязательны");
            }

            var result = await _userService.UpdatePasswordUser(changePassword);
            if (!result)
                return BadRequest("Неверный старый пароль");

            return Ok("Пароль успешно изменен");
        }
    }
} 