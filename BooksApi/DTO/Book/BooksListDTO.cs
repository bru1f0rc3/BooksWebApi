namespace BooksApi.DTO.Books
{
    public class BooksListDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Fragment { get; set; }
        public required string Cover_Link { get; set; }
        public required string Branch { get; set; }
        public required string Author { get; set; }
        public required string Category { get; set; }
    }
} 