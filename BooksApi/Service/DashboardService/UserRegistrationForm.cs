using Supabase;
using BooksApi.Models.Dashboard;

namespace BooksApi.Service.DashboardService
{
    public class UserRegistrationForm
    {
        private readonly Client _supabaseClient;

        public UserRegistrationForm(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task AddNewUser(Account account)
        {
            try
            {
                await _supabaseClient.From<Account>().Insert(account);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}