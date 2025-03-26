namespace BooksApi.DTO.EventBook
{
    public class BookEventHistoryDTO
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string UserLogin { get; set; }
        public string UserFullName { get; set; }
        public string EventType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 