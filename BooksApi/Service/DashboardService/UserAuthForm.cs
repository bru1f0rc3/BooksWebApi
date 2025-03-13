using BooksApi.Models.Dashboard;

namespace BooksApi.Service.DashboardService
{
    public class UserAuthForm
    {
        private readonly Supabase.Client _supabaseClient;

        public UserAuthForm(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<Account?> SignTask(Account account)
        {
            try
            {
                var query = await _supabaseClient
                    .From<Account>()
                    .Select("*")
                    .Match(new Dictionary<string, string>
                    {
                        { "login", account.Login },
                        { "password", account.Password }
                    })
                    .Single();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
