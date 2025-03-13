using BooksApi.DTO.Dashboard;
using BooksApi.Models.Dashboard;
using BooksApi.Service.DashboardService;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers.Dashboard;

[Route("api/user/register")]
[ApiController]
public class RegisterrAccountController : ControllerBase
{
    private readonly UserRegistrationForm _form;
    public RegisterrAccountController(UserRegistrationForm form)
    {
        _form = form;
    }
    [HttpPost]
    public async Task<IActionResult> RegisterUserTask([FromBody] AccountsDTO userDto)
    {
        var newUser = new Account
        {
            Login = userDto.Login,
            Password = userDto.Password,
            FullName = userDto.FullName,
            Phone = userDto.Phone,
            Email = userDto.Email,
            Role = 3
        };
        try
        {
            await _form.AddNewUser(newUser);
            return Ok(new { message = "Пользователь успешно зарегистрирован" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка регистрации", error = ex.Message });
        }
    }
}