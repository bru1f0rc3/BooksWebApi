using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BooksApi.Models.Book
{
    [Table("Books")]
    public class Bookss : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("title")]
        public string? Title { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("fragment")]
        public string? Fragment { get; set; }

        [Column("cover_link")]
        public string? Cover_Link { get; set; }

        [Column("branch_id")]
        public int Branch { get; set; }

        [Column("author_id")]
        public int Author { get; set; }

        [Column("category_id")]
        public int Categories { get; set; }
    }
}