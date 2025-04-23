namespace WebApplication2.DTO.BookEvent
{
    public class UserBookHistoryDTO
    {
        public int book_id { get; set; }
        public string book_title { get; set; } = string.Empty;
        public string author_name { get; set; } = string.Empty;
        public string category_name { get; set; } = string.Empty;
        public string branch_name { get; set; } = string.Empty;
        public DateTime last_event_date { get; set; }
        public string last_event_type { get; set; } = string.Empty;
    }
}
