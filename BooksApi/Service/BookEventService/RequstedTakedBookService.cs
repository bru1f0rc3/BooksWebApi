using BooksApi.DTO.EventBook;
using BooksApi.Models.EventBook;
using Supabase;

namespace BooksApi.Controllers.BookEventController
{
    public class RequstedTakedBookService
    {
        private readonly Client _supabaseClient;

        public RequstedTakedBookService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task RequestBook(Requested_Books requests)
        {
            var query = await _supabaseClient.From<Requested_Books>().Insert(new Requested_Books
            {
                BookId = requests.BookId,
                AccountId = requests.AccountId,
            });
        }

        public async Task TakedBook(Taken_Books requests)
        {
            var query = await _supabaseClient.From<Taken_Books>().Insert(new Taken_Books
            {
                BookId = requests.BookId,
                AccountId = requests.AccountId,
            });
        }
    }
}
