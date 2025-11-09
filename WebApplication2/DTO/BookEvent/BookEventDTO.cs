namespace WebApplication2.DTO.BookEvent
{
    public class BookEventDTO
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int BookId { get; set; }
        public int EventTypeId { get; set; }
        public DateTime EventDate { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string EventTypeName { get; set; } = string.Empty;
    }
} 