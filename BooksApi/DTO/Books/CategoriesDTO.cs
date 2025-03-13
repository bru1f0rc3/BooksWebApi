using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BooksApi.DTO.Books
{
    [Table("Categories")]
    public class Categories : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }
        [Column("name")]
        public string? Name { get; set; }
    }
}
