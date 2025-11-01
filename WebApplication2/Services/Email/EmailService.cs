using System.Net;
using System.Net.Mail;
using WebApplication2.Interfaces;

namespace WebApplication2.Services.Email
{
    /// <summary>
    /// Сервис для отправки email сообщений через SMTP
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Отправить код верификации на email
        /// </summary>
        /// <param name="email">Email получателя</param>
        /// <param name="code">Код верификации</param>
        /// <returns>True если отправлено успешно</returns>
        public async Task<bool> SendVerificationCodeAsync(string email, string code)
        {
            try
            {
                var subject = "Код верификации - Library Management System";
                var body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; padding: 20px; background-color: #f5f5f5;'>
                        <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
                            <h2 style='color: #2c3e50; border-bottom: 3px solid #3498db; padding-bottom: 10px;'>📚 Библиотечная Система</h2>
                            <p style='font-size: 16px; color: #333;'>Здравствуйте!</p>
                            <p style='font-size: 16px; color: #333;'>Ваш код верификации для регистрации в системе:</p>
                            <div style='background-color: #ecf0f1; padding: 20px; text-align: center; border-radius: 5px; margin: 20px 0;'>
                                <h1 style='color: #3498db; font-size: 48px; margin: 0; letter-spacing: 10px;'>{code}</h1>
                            </div>
                            <p style='font-size: 14px; color: #7f8c8d;'>Код действителен в течение <strong>15 минут</strong>.</p>
                            <p style='font-size: 14px; color: #7f8c8d;'>Если вы не запрашивали этот код, просто проигнорируйте это письмо.</p>
                            <hr style='border: none; border-top: 1px solid #ecf0f1; margin: 30px 0;'>
                            <p style='font-size: 12px; color: #95a5a6; text-align: center;'>
                                © 2024 Library Management System. Все права защищены.
                            </p>
                        </div>
                    </body>
                    </html>
                ";

                return await SendEmailAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при отправке кода верификации на {email}");
                return false;
            }
        }

        /// <summary>
        /// Генерировать случайный 6-значный код
        /// </summary>
        /// <returns>Код верификации</returns>
        public string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        /// <summary>
        /// Отправить приветственное письмо
        /// </summary>
        /// <param name="email">Email получателя</param>
        /// <param name="fullName">Полное имя пользователя</param>
        /// <returns>True если отправлено успешно</returns>
        public async Task<bool> SendWelcomeEmailAsync(string email, string fullName)
        {
            try
            {
                var subject = "Добро пожаловать в Library Management System! 🎉";
                var body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; padding: 20px; background-color: #f5f5f5;'>
                        <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
                            <h2 style='color: #2c3e50; border-bottom: 3px solid #27ae60; padding-bottom: 10px;'>🎉 Добро пожаловать!</h2>
                            <p style='font-size: 16px; color: #333;'>Здравствуйте, <strong>{(string.IsNullOrWhiteSpace(fullName) ? "Уважаемый пользователь" : fullName)}</strong>!</p>
                            <p style='font-size: 16px; color: #333;'>Спасибо за регистрацию в нашей библиотечной системе!</p>
                            <div style='background-color: #e8f8f5; padding: 20px; border-left: 4px solid #27ae60; margin: 20px 0;'>
                                <h3 style='color: #27ae60; margin-top: 0;'>Что вы можете делать:</h3>
                                <ul style='color: #333;'>
                                    <li>📖 Просматривать каталог книг</li>
                                    <li>🔖 Бронировать книги</li>
                                    <li>📚 Просматривать историю чтения</li>
                                    <li>⭐ Добавлять книги в избранное</li>
                                    <li>📊 Просматривать свою статистику</li>
                                </ul>
                            </div>
                            <p style='font-size: 14px; color: #7f8c8d;'>Ваш аккаунт успешно создан и готов к использованию!</p>
                            <hr style='border: none; border-top: 1px solid #ecf0f1; margin: 30px 0;'>
                            <p style='font-size: 12px; color: #95a5a6; text-align: center;'>
                                © 2025 Library Management System. Все права защищены.
                            </p>
                        </div>
                    </body>
                    </html>
                ";

                return await SendEmailAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при отправке приветственного письма на {email}");
                return false;
            }
        }

        /// <summary>
        /// Отправить уведомление о смене пароля
        /// </summary>
        /// <param name="email">Email получателя</param>
        /// <returns>True если отправлено успешно</returns>
        public async Task<bool> SendPasswordChangedNotificationAsync(string email)
        {
            try
            {
                var subject = "Ваш пароль был изменен";
                var body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; padding: 20px; background-color: #f5f5f5;'>
                        <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
                            <h2 style='color: #2c3e50; border-bottom: 3px solid #e67e22; padding-bottom: 10px;'>🔐 Безопасность аккаунта</h2>
                            <p style='font-size: 16px; color: #333;'>Здравствуйте!</p>
                            <p style='font-size: 16px; color: #333;'>Ваш пароль был успешно изменен.</p>
                            <div style='background-color: #fef5e7; padding: 20px; border-left: 4px solid #e67e22; margin: 20px 0;'>
                                <p style='color: #333; margin: 0;'><strong>⚠️ Важно:</strong></p>
                                <p style='color: #333;'>Если вы не меняли пароль, немедленно свяжитесь с администратором системы.</p>
                            </div>
                            <p style='font-size: 14px; color: #7f8c8d;'>Дата изменения: {DateTime.Now:dd.MM.yyyy HH:mm}</p>
                            <hr style='border: none; border-top: 1px solid #ecf0f1; margin: 30px 0;'>
                            <p style='font-size: 12px; color: #95a5a6; text-align: center;'>
                                © 2024 Library Management System. Все права защищены.
                            </p>
                        </div>
                    </body>
                    </html>
                ";

                return await SendEmailAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при отправке уведомления о смене пароля на {email}");
                return false;
            }
        }

        /// <summary>
        /// Базовый метод для отправки email через SMTP
        /// </summary>
        private async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpHost = _configuration["Email:SmtpHost"];
                var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
                var smtpUser = _configuration["Email:SmtpUser"];
                var smtpPassword = _configuration["Email:SmtpPassword"];
                var fromEmail = _configuration["Email:FromEmail"];
                var fromName = _configuration["Email:FromName"];
                var enableSsl = bool.Parse(_configuration["Email:EnableSsl"] ?? "true");

                if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPassword))
                {
                    _logger.LogWarning("Email настройки не сконфигурированы");
                    return false;
                }

                using var smtpClient = new SmtpClient(smtpHost)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(smtpUser, smtpPassword),
                    EnableSsl = enableSsl,
                    Timeout = 30000 // 30 секунд
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail ?? smtpUser, fromName ?? "Library Management System"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);

                _logger.LogInformation($"Email успешно отправлен на {toEmail}");
                return true;
            }
            catch (SmtpException smtpEx)
            {
                _logger.LogError(smtpEx, $"SMTP ошибка при отправке email на {toEmail}: {smtpEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Общая ошибка при отправке email на {toEmail}");
                return false;
            }
        }
    }
}
