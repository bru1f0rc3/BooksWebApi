using Supabase;
using BooksApi.Models.EventBook;
using BooksApi.DTO.EventBook;

namespace BooksApi.Service.BookEventService
{
    public class BookEventService
    {
        private readonly Client _supabaseClient;

        public BookEventService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<List<BookEventDTO>> GetUserBookEvents(int accountId)
        {
            var events = new List<BookEventDTO>();

            // Получаем сохраненные книги
            var savedBooks = await _supabaseClient
                .From<Saved_Books>()
                .Select("*")
                .Where(x => x.AccountId == accountId)
                .Get();

            events.AddRange(savedBooks.Models.Select(b => new BookEventDTO
            {
                Id = b.Id,
                BookId = b.BookId,
                AccountId = b.AccountId,
                EventType = "Saved",
                CreatedAt = b.CreatedAt
            }));

            // Получаем взятые книги
            var takenBooks = await _supabaseClient
                .From<Taken_Books>()
                .Select("*")
                .Where(x => x.AccountId == accountId)
                .Get();

            events.AddRange(takenBooks.Models.Select(b => new BookEventDTO
            {
                Id = b.Id,
                BookId = b.BookId,
                AccountId = b.AccountId,
                EventType = "Taken",
                CreatedAt = b.CreatedAt
            }));

            // Получаем запрошенные книги
            var requestedBooks = await _supabaseClient
                .From<Requested_Books>()
                .Select("*")
                .Where(x => x.AccountId == accountId)
                .Get();

            events.AddRange(requestedBooks.Models.Select(b => new BookEventDTO
            {
                Id = b.Id,
                BookId = b.BookId,
                AccountId = b.AccountId,
                EventType = "Requested",
                CreatedAt = b.CreatedAt
            }));

            // Получаем возвращенные книги
            var returnedBooks = await _supabaseClient
                .From<Returned_Books>()
                .Select("*")
                .Where(x => x.AccountId == accountId)
                .Get();

            events.AddRange(returnedBooks.Models.Select(b => new BookEventDTO
            {
                Id = b.Id,
                BookId = b.BookId,
                AccountId = b.AccountId,
                EventType = "Returned",
                CreatedAt = b.CreatedAt
            }));

            return events.OrderByDescending(e => e.CreatedAt).ToList();
        }

        public async Task<List<BookEventDTO>> GetAllBookEvents()
        {
            var events = new List<BookEventDTO>();

            // Получаем все события
            var savedBooks = await _supabaseClient.From<Saved_Books>().Select("*").Get();
            var takenBooks = await _supabaseClient.From<Taken_Books>().Select("*").Get();
            var requestedBooks = await _supabaseClient.From<Requested_Books>().Select("*").Get();
            var returnedBooks = await _supabaseClient.From<Returned_Books>().Select("*").Get();

            events.AddRange(savedBooks.Models.Select(b => new BookEventDTO
            {
                Id = b.Id,
                BookId = b.BookId,
                AccountId = b.AccountId,
                EventType = "Saved",
                CreatedAt = b.CreatedAt
            }));

            events.AddRange(takenBooks.Models.Select(b => new BookEventDTO
            {
                Id = b.Id,
                BookId = b.BookId,
                AccountId = b.AccountId,
                EventType = "Taken",
                CreatedAt = b.CreatedAt
            }));

            events.AddRange(requestedBooks.Models.Select(b => new BookEventDTO
            {
                Id = b.Id,
                BookId = b.BookId,
                AccountId = b.AccountId,
                EventType = "Requested",
                CreatedAt = b.CreatedAt
            }));

            events.AddRange(returnedBooks.Models.Select(b => new BookEventDTO
            {
                Id = b.Id,
                BookId = b.BookId,
                AccountId = b.AccountId,
                EventType = "Returned",
                CreatedAt = b.CreatedAt
            }));

            return events.OrderByDescending(e => e.CreatedAt).ToList();
        }
    }
} 