using BooksApi.Models.Book;
using BooksApi.DTO.Books;
using Supabase;

namespace BooksApi.Service.BookManagement
{
    public class EditBookService
    {
        private readonly Client _supabaseClient;

        public EditBookService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient ?? throw new ArgumentNullException(nameof(supabaseClient));
        }

        public async Task<Bookss> EditBookTask(BooksDTO bookDto)
        {
            try
            {
                if (bookDto == null)
                {
                    throw new ArgumentNullException(nameof(bookDto));
                }

                if (bookDto.Id <= 0)
                {
                    throw new ArgumentException("Некорректный ID книги", nameof(bookDto.Id));
                }

                var book = new Bookss
                {
                    Id = bookDto.Id,
                    Title = bookDto.Title,
                    Description = bookDto.Description,
                    Fragment = bookDto.Fragment,
                    Cover_Link = bookDto.Cover_Link,
                    Branch = bookDto.Branch,
                    Author = bookDto.Author,
                    Categories = bookDto.Categories
                };

                var result = await _supabaseClient
                    .From<Bookss>()
                    .Match(new Dictionary<string, string> { { "id", book.Id.ToString() } })
                    .Update(book);
                
                return result.Models.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при редактировании книги: " + ex.Message);
            }
        }
    }
} 