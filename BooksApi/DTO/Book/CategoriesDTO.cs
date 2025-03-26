using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BooksApi.DTO.Books
{
    public class CategoriesDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
