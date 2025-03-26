using BooksApi.Models.Dashboard;
using Supabase;
using BooksApi.Service;
using BooksApi.DTO.Dashboard;

namespace BooksApi.Service.DashboardService
{
    public class UserAuthForm
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly JwtService _jwtService;

        public UserAuthForm(Supabase.Client supabaseClient, JwtService jwtService)
        {
            _supabaseClient = supabaseClient ?? throw new ArgumentNullException(nameof(supabaseClient));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        }

        public async Task<Account> SignTask(Account account)
        {
            try
            {
                if (account == null)
                {
                    throw new ArgumentNullException(nameof(account));
                }

                var query = await _supabaseClient
                    .From<Account>()
                    .Select("*")
                    .Where(x => x.Login == account.Login)
                    .Single();

                if (query != null && query.Password == account.Password)
                {
                    return query;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<AuthResponseDto> ValidateTokenAndGetUser(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return new AuthResponseDto 
                    { 
                        Token = string.Empty,
                        User = new AccountsDTO(),
                        Message = "Токен не предоставлен" 
                    };
                }

                if (!_jwtService.ValidateToken(token))
                {
                    return new AuthResponseDto 
                    { 
                        Token = string.Empty,
                        User = new AccountsDTO(),
                        Message = "Недействительный токен" 
                    };
                }

                if (_jwtService.IsTokenExpired(token))
                {
                    return new AuthResponseDto 
                    { 
                        Token = string.Empty,
                        User = new AccountsDTO(),
                        Message = "Токен истек" 
                    };
                }

                var userId = _jwtService.GetUserIdFromToken(token);
                var userRole = _jwtService.GetUserRoleFromToken(token);

                var query = await _supabaseClient
                    .From<Account>()
                    .Select("*")
                    .Match(new Dictionary<string, string> { { "id", userId.ToString() } })
                    .Single();

                if (query != null)
                {
                    return new AuthResponseDto
                    {
                        Token = token,
                        User = new AccountsDTO
                        {
                            Id = query.Id,
                            Login = query.Login,
                            FullName = query.FullName ?? string.Empty,
                            Email = query.Email ?? string.Empty,
                            Phone = query.Phone ?? string.Empty,
                            Role = query.Role
                        },
                        Message = "Токен действителен"
                    };
                }

                return new AuthResponseDto 
                { 
                    Token = string.Empty,
                    User = new AccountsDTO(),
                    Message = "Пользователь не найден" 
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto 
                { 
                    Token = string.Empty,
                    User = new AccountsDTO(),
                    Message = $"Ошибка валидации токена: {ex.Message}" 
                };
            }
        }
    }
}
