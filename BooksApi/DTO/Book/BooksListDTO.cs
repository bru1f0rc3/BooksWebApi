namespace BooksApi.DTO.Books
{
    public class BooksListDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Fragment { get; set; }
        public string Cover_Link { get; set; }
        public string Branch { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
    }
} 