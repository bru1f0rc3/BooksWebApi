using BooksApi.Models.Book;
using BooksApi.DTO.Books;
using Supabase;

namespace BooksApi.Service.BookManagement
{
    public class AddBookService
    {
        private readonly Client _supabaseClient;

        public AddBookService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<Bookss> AddBookTask(BooksDTO bookDto)
        {
            try
            {
                var book = new Bookss
                {
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
                    .Insert(book);
                
                return result.Models.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при добавлении книги: " + ex.Message);
            }
        }
    }
} 