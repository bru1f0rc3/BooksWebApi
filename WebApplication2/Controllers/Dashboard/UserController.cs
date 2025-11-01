using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO.Auth;
using WebApplication2.DTO.Dashboard;
using WebApplication2.Interfaces;

namespace WebApplication2.Controllers.Dashboard
{
    /// <summary>
    /// Контроллер для управления пользователями системы
    /// </summary>
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Конструктор контроллера пользователей
        /// </summary>
        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        /// <summary>
        /// Изменить email пользователя с проверкой кода верификации
        /// Код должен быть отправлен на НОВЫЙ email
        /// </summary>
        [HttpPut("change-email")]
        public async Task<ActionResult> ChangeEmail(ChangeEmailUserDTO changeEmail)
        {
            if (string.IsNullOrEmpty(changeEmail.email) || string.IsNullOrEmpty(changeEmail.code))
            {
                return BadRequest("Email и код верификации обязательны");
            }

            // Проверяем код верификации для НОВОГО email
            var isValid = await _userService.VerifyCodeAsync(changeEmail.email, changeEmail.code);
            if (!isValid)
            {
                return BadRequest("Неверный или истекший код верификации");
            }

            // Проверяем, не занят ли новый email другим пользователем
            var emailExists = await _userService.EmailExistsAsync(changeEmail.email);
            if (emailExists)
            {
                return BadRequest("Этот email уже используется другим пользователем");
            }

            // Обновляем email
            var result = await _userService.UpdateEmailUser(changeEmail);
            if (!result)
            {
                return BadRequest("Не удалось обновить email");
            }

            return Ok(new { message = "Email успешно изменен" });
        }

        /// <summary>
        /// Изменить пароль пользователя с проверкой кода верификации
        /// </summary>
        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDTO changePassword)
        {
            if (string.IsNullOrEmpty(changePassword.old_password) || 
                string.IsNullOrEmpty(changePassword.new_password) ||
                string.IsNullOrEmpty(changePassword.email) ||
                string.IsNullOrEmpty(changePassword.code))
            {
                return BadRequest("Все поля обязательны для заполнения");
            }

            // Проверяем код верификации
            var isValid = await _userService.VerifyCodeAsync(changePassword.email, changePassword.code);
            if (!isValid)
            {
                return BadRequest("Неверный или истекший код верификации");
            }

            // Обновляем пароль
            var result = await _userService.UpdatePasswordUser(changePassword);
            if (!result)
            {
                return BadRequest("Неверный старый пароль");
            }

            await _emailService.SendPasswordChangedNotificationAsync(changePassword.email);

            return Ok(new { message = "Пароль успешно изменен" });
        }

        /// <summary>
        /// Отправить код верификации на email для регистрации
        /// </summary>
        /// <param name="request">Email для отправки кода</param>
        /// <returns>Статус отправки</returns>
        [HttpPost("send-verification-code")]
        public async Task<ActionResult> SendVerificationCode([FromBody] SendVerificationCodeDTO request)
        {
            if (string.IsNullOrEmpty(request.email))
            {
                return BadRequest("Email обязателен");
            }

            var emailExists = await _userService.EmailExistsAsync(request.email);
            if (emailExists)
            {
                return BadRequest("Этот email уже зарегистрирован");
            }

            var code = _emailService.GenerateVerificationCode();

            var saved = await _userService.SaveVerificationCodeAsync(request.email, code);
            if (!saved)
            {
                return StatusCode(500, "Не удалось сохранить код верификации");
            }

            var sent = await _emailService.SendVerificationCodeAsync(request.email, code);
            if (!sent)
            {
                return StatusCode(500, "Не удалось отправить email. Проверьте настройки SMTP");
            }

            return Ok(new
            {
                message = "Код верификации отправлен на ваш email",
                expiresIn = "15 минут"
            });
        }

        /// <summary>
        /// Отправить код верификации на email (для смены email или пароля)
        /// Для смены пароля - отправить на текущий email
        /// Для смены email - отправить на НОВЫЙ email
        /// </summary>
        /// <param name="request">Email для отправки кода</param>
        /// <returns>Статус отправки</returns>
        [HttpPost("send-verification-code-for-change")]
        public async Task<ActionResult> SendVerificationCodeForChange([FromBody] SendVerificationCodeDTO request)
        {
            if (string.IsNullOrEmpty(request.email))
            {
                return BadRequest("Email обязателен");
            }

            var code = _emailService.GenerateVerificationCode();

            var saved = await _userService.SaveVerificationCodeAsync(request.email, code);
            if (!saved)
            {
                return StatusCode(500, "Не удалось сохранить код верификации");
            }

            var sent = await _emailService.SendVerificationCodeAsync(request.email, code);
            if (!sent)
            {
                return StatusCode(500, "Не удалось отправить email. Проверьте настройки SMTP");
            }

            return Ok(new
            {
                message = "Код верификации отправлен на указанный email",
                expiresIn = "15 минут"
            });
        }

        /// <summary>
        /// Проверить код верификации
        /// </summary>
        /// <param name="request">Email и код для проверки</param>
        /// <returns>Результат проверки</returns>
        [HttpPost("verify-code")]
        public async Task<ActionResult> VerifyCode([FromBody] VerifyCodeDTO request)
        {
            if (string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.code))
            {
                return BadRequest("Email и код обязательны");
            }

            var isValid = await _userService.VerifyCodeAsync(request.email, request.code);
            if (!isValid)
            {
                return BadRequest("Неверный или истекший код");
            }

            return Ok(new
            {
                message = "Email успешно верифицирован",
                verified = true
            });
        }

        /// <summary>
        /// Регистрация нового пользователя (требуется верификация email)
        /// </summary>
        /// <param name="request">Данные для регистрации включая код верификации</param>
        /// <returns>Результат регистрации</returns>
        [HttpPost("register-with-verification")]
        public async Task<ActionResult> RegisterWithVerification([FromBody] RegisterWithVerificationDTO request)
        {
            if (string.IsNullOrEmpty(request.email) || 
                string.IsNullOrEmpty(request.code) ||
                string.IsNullOrEmpty(request.login) || 
                string.IsNullOrEmpty(request.password))
            {
                return BadRequest("Email, код верификации, логин и пароль обязательны");
            }

            if (string.IsNullOrWhiteSpace(request.full_name))
            {
                return BadRequest("Полное имя (full_name) обязательно для заполнения");
            }

            var isValid = await _userService.VerifyCodeAsync(request.email, request.code);
            if (!isValid)
            {
                return BadRequest("Неверный или истекший код верификации");
            }

            var emailExists = await _userService.EmailExistsAsync(request.email);
            if (emailExists)
            {
                return BadRequest("Этот email уже зарегистрирован");
            }

            var createUser = new CreateAccountDTO
            {
                login = request.login,
                password = request.password,
                full_name = request.full_name,
                phone = request.phone,
                email = request.email
            };

            await _userService.CreateUserAsync(createUser);

            await _emailService.SendWelcomeEmailAsync(request.email, request.full_name);

            return Ok(new
            {
                message = "Пользователь успешно зарегистрирован",
                email = request.email
            });
        }
    }
} 