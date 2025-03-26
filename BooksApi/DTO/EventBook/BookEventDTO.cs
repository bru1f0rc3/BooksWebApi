namespace BooksApi.DTO.EventBook
{
    public class BookEventDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int AccountId { get; set; }
        public string EventType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string BookTitle { get; set; }
        public string UserName { get; set; }
    }
} 