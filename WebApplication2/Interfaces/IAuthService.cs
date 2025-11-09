using WebApplication2.DTO.Auth;

namespace WebApplication2.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для аутентификации и авторизации пользователей
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Аутентифицировать пользователя и сгенерировать JWT токен
        /// </summary>
        /// <param name="login">Данные для входа (логин и пароль)</param>
        /// <returns>Токен и информация о пользователе при успешной аутентификации, иначе null</returns>
        Task<TokenDTO?> Login(LoginDTO login);
    }
}
