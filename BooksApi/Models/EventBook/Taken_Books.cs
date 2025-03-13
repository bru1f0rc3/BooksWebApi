using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BooksApi.Models.EventBook
{
    [Table("Taken_Books")]
    public class Taken_Books : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }
        [Column("account_id")]
        public int AccountId { get; set; }
        [Column("book_id")]
        public int BookId { get; set; }
    }
}
