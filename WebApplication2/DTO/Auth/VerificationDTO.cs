namespace WebApplication2.DTO.Auth
{
    /// <summary>
    /// DTO для запроса кода верификации
    /// </summary>
    public class SendVerificationCodeDTO
    {
        /// <summary>
        /// Email для отправки кода
        /// </summary>
        public string email { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO для проверки кода верификации
    /// </summary>
    public class VerifyCodeDTO
    {
        /// <summary>
        /// Email пользователя
        /// </summary>
        public string email { get; set; } = string.Empty;

        /// <summary>
        /// Код верификации
        /// </summary>
        public string code { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO для регистрации с верифицированным email
    /// </summary>
    public class RegisterWithVerificationDTO
    {
        /// <summary>
        /// Email (должен быть верифицирован)
        /// </summary>
        public string email { get; set; } = string.Empty;

        /// <summary>
        /// Код верификации
        /// </summary>
        public string code { get; set; } = string.Empty;

        /// <summary>
        /// Логин
        /// </summary>
        public string login { get; set; } = string.Empty;

        /// <summary>
        /// Пароль
        /// </summary>
        public string password { get; set; } = string.Empty;

        /// <summary>
        /// Полное имя
        /// </summary>
        public string full_name { get; set; } = string.Empty;

        /// <summary>
        /// Телефон
        /// </summary>
        public string phone { get; set; } = string.Empty;
    }
}
