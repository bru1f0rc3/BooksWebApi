using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BooksApi.Models.EventBook
{
    [Table("Requested_Books")]
    public class Requested_Books : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }
        [Column("account_id")]
        public int AccountId { get; set; }
        [Column("book_id")]
        public int BookId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
