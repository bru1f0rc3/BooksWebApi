using Supabase;
using BooksApi.DTO.Books;
using BooksApi.Models.Book;

namespace BooksApi.Service.BookService
{
    public class ListedBookService
    {
        private readonly Client _supabaseClient;

        public ListedBookService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<List<BooksDTO>> ListedBookTask()
        {
            try
            {
                var query = await _supabaseClient
                    .From<Books>()
                    .Select("*")
                    .Get();

                return query.Models.Select(book => new BooksDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    Fragment = book.Fragment,
                    Cover_Link = book.Cover_Link,
                    Branch = book.Branch,
                    Author = book.Author,
                    Categories = book.Categories
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка книг: " + ex.Message, ex);
            }
        }
    }
}