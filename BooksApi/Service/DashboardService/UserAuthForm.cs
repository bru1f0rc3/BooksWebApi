using BooksApi.Models.Dashboard;
using Supabase;

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
    }
}
