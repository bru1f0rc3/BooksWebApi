using BooksApi.Models.EventBook;
using Supabase;

namespace BooksApi.Controllers.BookEventController
{
    public class ReturnedSavedBookService
    {
        private readonly Client _supabaseClient;

        public ReturnedSavedBookService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task ReturnedBook(Returned_Books requests)
        {
            var query = await _supabaseClient.From<Returned_Books>().Insert(new Returned_Books
            {
                BookId = requests.BookId,
                AccountId = requests.AccountId,
            });
        }

        public async Task SavedBook(Saved_Books requests)
        {
            var query = await _supabaseClient.From<Saved_Books>().Insert(new Saved_Books
            {
                BookId = requests.BookId,
                AccountId = requests.AccountId,
            });
        }
    }
}
