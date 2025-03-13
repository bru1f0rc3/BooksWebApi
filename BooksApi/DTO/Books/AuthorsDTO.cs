using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BooksApi.DTO.Books
{
    [Table("Authors")]
    public class Authors : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }
        [Column("full_name")]
        public string? FullName { get; set; }
    }
}
