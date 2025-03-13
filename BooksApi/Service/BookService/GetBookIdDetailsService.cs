using Supabase;
using BooksApi.DTO.Books;
using BooksApi.Models.Book;

namespace BooksApi.Service.BookService
{
    public class GetBookIdDetailsService
    {
        private readonly Client _supabaseClient;

        public GetBookIdDetailsService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<List<BooksDTO>> GetBookDetailsTask(int id)
        {
            try
            {
                var query = await _supabaseClient
                    .From<Books>()
                    .Select("*")
                    .Match(new Dictionary<string, string>
                    {
                        { "id", id.ToString() }
                    })
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
                throw new Exception("Ошибка при загрузке книги: " + ex.Message, ex);
            }
        }
    }
}