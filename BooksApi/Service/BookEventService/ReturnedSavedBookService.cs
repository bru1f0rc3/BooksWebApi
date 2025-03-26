using BooksApi.Models.EventBook;
using Supabase;
using BooksApi.DTO.EventBook;

namespace BooksApi.Service.BookEventService
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
            try
            {
                await _supabaseClient.From<Returned_Books>().Insert(new Returned_Books
                {
                    BookId = requests.BookId,
                    AccountId = requests.AccountId,
                    CreatedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при возврате книги: {ex.Message}");
            }
        }

        public async Task SavedBook(Saved_Books requests)
        {
            try
            {
                await _supabaseClient.From<Saved_Books>().Insert(new Saved_Books
                {
                    BookId = requests.BookId,
                    AccountId = requests.AccountId,
                    CreatedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при сохранении книги: {ex.Message}");
            }
        }

        public async Task<List<Saved_Books>> GetUserSavedBooks(int accountId)
        {
            try
            {
                var result = await _supabaseClient
                    .From<Saved_Books>()
                    .Select("*")
                    .Where(x => x.AccountId == accountId)
                    .Get();

                return result.Models;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении сохраненных книг: {ex.Message}");
            }
        }
    }
}
