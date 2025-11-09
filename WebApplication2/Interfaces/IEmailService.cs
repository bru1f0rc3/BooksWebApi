namespace WebApplication2.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для отправки email сообщений
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Отправить код верификации на email
        /// </summary>
        /// <param name="email">Email получателя</param>
        /// <param name="code">Код верификации</param>
        /// <returns>True если отправлено успешно</returns>
        Task<bool> SendVerificationCodeAsync(string email, string code);

        /// <summary>
        /// Генерировать случайный 6-значный код
        /// </summary>
        /// <returns>Код верификации</returns>
        string GenerateVerificationCode();

        /// <summary>
        /// Отправить приветственное письмо
        /// </summary>
        /// <param name="email">Email получателя</param>
        /// <param name="fullName">Полное имя пользователя</param>
        /// <returns>True если отправлено успешно</returns>
        Task<bool> SendWelcomeEmailAsync(string email, string fullName);

        /// <summary>
        /// Отправить уведомление о смене пароля
        /// </summary>
        /// <param name="email">Email получателя</param>
        /// <returns>True если отправлено успешно</returns>
        Task<bool> SendPasswordChangedNotificationAsync(string email);
    }
}
