namespace BooksApi.DTO.EventBook
{
    public class Taken_BooksDTO
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int BookId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
