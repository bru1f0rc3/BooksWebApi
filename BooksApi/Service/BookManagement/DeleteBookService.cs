using BooksApi.Models.Book;
using Supabase;

namespace BooksApi.Service.BookManagement
{
    public class DeleteBookService
    {
        private readonly Client _supabaseClient;

        public DeleteBookService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient ?? throw new ArgumentNullException(nameof(supabaseClient));
        }

        public async Task<bool> DeleteBookTask(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Некорректный ID книги", nameof(id));
                }

                await _supabaseClient
                    .From<Bookss>()
                    .Match(new Dictionary<string, string> { { "id", id.ToString() } })
                    .Delete();
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при удалении книги: " + ex.Message);
            }
        }
    }
} 