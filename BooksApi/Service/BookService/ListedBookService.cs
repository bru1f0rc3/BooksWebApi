using Supabase;
using BooksApi.DTO.Books;
using BooksApi.Models.Book;
using BooksApi.Models.Dashboard;

namespace BooksApi.Service.BookService
{
    public class ListedBookService
    {
        private readonly Client _supabaseClient;

        public ListedBookService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient ?? throw new ArgumentNullException(nameof(supabaseClient));
        }

        public async Task<List<BooksListDTO>> ListedBookTask()
        {
            var query = await _supabaseClient
                .From<Bookss>()
                .Get();

            var booksList = new List<BooksListDTO>();

            foreach (var book in query.Models)
            {
                // Получаем информацию об авторе
                var author = await _supabaseClient
                    .From<Authors>()
                    .Match(new Dictionary<string, string> { { "id", book.Author.ToString() } })
                    .Get();

                // Получаем информацию о категории
                var category = await _supabaseClient
                    .From<Categories>()
                    .Match(new Dictionary<string, string> { { "id", book.Categories.ToString() } })
                    .Get();

                // Получаем информацию о филиале
                var branch = await _supabaseClient
                    .From<Branches>()
                    .Match(new Dictionary<string, string> { { "id", book.Branch.ToString() } })
                    .Get();

                booksList.Add(new BooksListDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    Fragment = book.Fragment,
                    Cover_Link = book.Cover_Link,
                    Branch = branch.Models.Count > 0 ? branch.Models[0].Name : "Неизвестный филиал",
                    Author = author.Models.Count > 0 ? author.Models[0].FullName : "Неизвестный автор",
                    Category = category.Models.Count > 0 ? category.Models[0].Name : "Неизвестная категория"
                });
            }

            return booksList;
        }
    }
}