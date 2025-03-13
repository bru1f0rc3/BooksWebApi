using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BooksApi.Models.Book
{
    [Table("Authors")]
    public class AuthorsDTO : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }
        [Column("full_name")]
        public string? FullName { get; set; }
    }
}
