using WebApplication2.Connection;
using WebApplication2.DTO.Dashboard;

namespace WebApplication2.Services.Dashboard
{
    public class UserService
    {
        public async Task CreateUserAsync(CreateAccountDTO user)
        {
            const string sql = @"
                INSERT INTO ""Accounts"" (login, password, full_name, phone, email) 
                VALUES (@login, @password, @full_name, @phone, @email)";

            await DbConnect.ExecuteAsync(sql, user);
        }

        public async Task<bool> UpdatePasswordUser(ChangePasswordDTO user)
        {
            const string checkSql = @"
                SELECT COUNT(1) 
                FROM ""Accounts"" 
                WHERE id = @id 
                AND password = @old_password";

            var exists = await DbConnect.QueryFirstOrDefaultAsync<int>(checkSql,
                new { user.id, user.old_password });

            if (exists == 0)
                return false;

            const string sql = @"
                UPDATE ""Accounts"" 
                SET password = @new_password
                WHERE id = @id";
            var rowsAffected = await DbConnect.ExecuteAsync(sql,
                            new { user.id, user.new_password });
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateEmailUser(ChangeEmailUserDTO user)
        {
            const string sql = @"
                UPDATE ""Accounts"" 
                SET email = @email
                WHERE id = @id";
            var rowsAffected = await DbConnect.ExecuteAsync(sql,
                            new { user.id, user.email });
            return rowsAffected > 0;
        }
    }
}