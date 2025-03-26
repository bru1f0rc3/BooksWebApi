using BooksApi.DTO.EventBook;
using BooksApi.Models.EventBook;
using Supabase;

namespace BooksApi.Service.BookEventService
{
    public class RequstedTakedBookService
    {
        private readonly Client _supabaseClient;

        public RequstedTakedBookService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient ?? throw new ArgumentNullException(nameof(supabaseClient));
        }

        public async Task RequestBook(Requested_Books requests)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }

            var query = await _supabaseClient.From<Requested_Books>().Insert(new Requested_Books
            {
                BookId = requests.BookId,
                AccountId = requests.AccountId,
            });
        }

        public async Task TakedBook(Taken_Books requests)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }

            var query = await _supabaseClient.From<Taken_Books>().Insert(new Taken_Books
            {
                BookId = requests.BookId,
                AccountId = requests.AccountId,
            });
        }
    }
}
