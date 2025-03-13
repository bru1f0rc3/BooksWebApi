using BooksApi.DTO.Books;
using BooksApi.Models.Book;
using Microsoft.AspNetCore.Mvc;
using Supabase;

namespace BooksApi.Service.BookService
{
    public class SearchService : ControllerBase
    {
        private readonly Client _supabaseClient;

        public SearchService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<List<BooksDTO>> SearchBooksTask(string query)
        {
            try
            {
                var result = await _supabaseClient
                    .From<Books>()
                    .Select("*")
                    .Where(x => x.Title.Contains(query)).Get();
                return result.Models.Select(book => new BooksDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    Categories = book.Categories
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<BooksDTO>();
            }
        }
    }
}
