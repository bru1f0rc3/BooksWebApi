using WebApplication2.DTO.Dashboard;

namespace WebApplication2.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с пользователями системы
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        /// <param name="user">Данные для создания пользователя</param>
        Task CreateUserAsync(CreateAccountDTO user);

        /// <summary>
        /// Обновить пароль пользователя
        /// </summary>
        /// <param name="user">Данные для смены пароля (старый и новый пароль)</param>
        /// <returns>True, если пароль был успешно изменен, иначе false</returns>
        Task<bool> UpdatePasswordUser(ChangePasswordDTO user);

        /// <summary>
        /// Обновить email пользователя
        /// </summary>
        /// <param name="user">Данные для изменения email</param>
        /// <returns>True, если email был успешно изменен, иначе false</returns>
        Task<bool> UpdateEmailUser(ChangeEmailUserDTO user);

        /// <summary>
        /// Сохранить код верификации в базу данных
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <param name="code">Код верификации</param>
        /// <returns>True если сохранено успешно</returns>
        Task<bool> SaveVerificationCodeAsync(string email, string code);

        /// <summary>
        /// Проверить код верификации
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <param name="code">Код верификации</param>
        /// <returns>True если код валидный</returns>
        Task<bool> VerifyCodeAsync(string email, string code);

        /// <summary>
        /// Проверить существование email
        /// </summary>
        /// <param name="email">Email для проверки</param>
        /// <returns>True если email уже существует</returns>
        Task<bool> EmailExistsAsync(string email);
    }
}
