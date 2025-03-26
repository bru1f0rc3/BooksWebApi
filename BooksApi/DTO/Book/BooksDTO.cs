using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BooksApi.DTO.Books
{
    public class BooksDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Fragment { get; set; }
        public required string Cover_Link { get; set; }
        public int Branch { get; set; }
        public int Author { get; set; }
        public int Categories { get; set; }
    }
}
