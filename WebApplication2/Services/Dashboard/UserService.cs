using WebApplication2.Connection;
using WebApplication2.DTO.Dashboard;
using WebApplication2.Interfaces;

namespace WebApplication2.Services.Dashboard
{
    /// <summary>
    /// Сервис для работы с пользователями системы
    /// </summary>
    public class UserService : IUserService
    {
        public async Task CreateUserAsync(CreateAccountDTO user)
        {
            const string sql = @"
                INSERT INTO accounts (login, password, full_name, phone, email, role_id) 
                VALUES (@login, @password, @full_name, @phone, @email, 3)";
         
            await DbConnect.ExecuteAsync(sql, user);
        }

        public async Task<bool> SaveVerificationCodeAsync(string email, string code)
        {
            const string deleteSql = @"
                DELETE FROM verification_codes 
                WHERE email = @email";
            await DbConnect.ExecuteAsync(deleteSql, new { email });

            const string insertSql = @"
                INSERT INTO verification_codes (email, code, expiry_date, created_at) 
                VALUES (@email, @code, @expiry_date, @created_at)";
            
            var rowsAffected = await DbConnect.ExecuteAsync(insertSql, new
            {
                email,
                code,
                expiry_date = DateTime.UtcNow.AddMinutes(15),
                created_at = DateTime.UtcNow
            });

            return rowsAffected > 0;
        }

        public async Task<bool> VerifyCodeAsync(string email, string code)
        {
            const string sql = @"
                SELECT COUNT(1) 
                FROM verification_codes 
                WHERE email = @email 
                AND code = @code 
                AND expiry_date > @now
                AND is_used = false";

            var isValid = await DbConnect.QueryFirstOrDefaultAsync<int>(sql, new
            {
                email,
                code,
                now = DateTime.UtcNow
            });

            if (isValid > 0)
            {
                const string updateSql = @"
                    UPDATE verification_codes 
                    SET is_used = true 
                    WHERE email = @email AND code = @code";
                await DbConnect.ExecuteAsync(updateSql, new { email, code });
                return true;
            }

            return false;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            const string sql = @"
                SELECT COUNT(1) 
                FROM accounts 
                WHERE email = @email";

            var count = await DbConnect.QueryFirstOrDefaultAsync<int>(sql, new { email });
            return count > 0;
        }

        public async Task<bool> UpdatePasswordUser(ChangePasswordDTO user)
        {
            const string checkSql = @"
                SELECT COUNT(1) 
                FROM accounts 
                WHERE id = @id 
                AND password = @old_password";

            var exists = await DbConnect.QueryFirstOrDefaultAsync<int>(checkSql,
                new { user.id, user.old_password });

            if (exists == 0)
                return false;

            const string sql = @"
                UPDATE accounts 
                SET password = @new_password
                WHERE id = @id";
            var rowsAffected = await DbConnect.ExecuteAsync(sql,
                            new { user.id, user.new_password });
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateEmailUser(ChangeEmailUserDTO user)
        {
            const string sql = @"
                UPDATE accounts 
                SET email = @email
                WHERE id = @id";
            var rowsAffected = await DbConnect.ExecuteAsync(sql,
                            new { user.id, user.email });
            return rowsAffected > 0;
        }
    }
} 