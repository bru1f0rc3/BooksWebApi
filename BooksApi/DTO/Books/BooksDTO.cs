using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BooksApi.DTO.Books
{
    public class BooksDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Fragment { get; set; }
        public string? Cover_Link { get; set; }
        public int Branch { get; set; }
        public int Author { get; set; }
        public int Categories { get; set; }
    }
}
