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
            _supabaseClient = supabaseClient;
        }

        public async Task<Bookss> EditBookTask(BooksDTO bookDto)
        {
            try
            {
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