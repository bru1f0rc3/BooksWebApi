namespace WebApplication2.DTO.BookEvent
{
    public class TakedBookDTO
    {
        public int book_event_id { get; set; }
        public int book_id { get; set; }
        public string book_title { get; set; }
        public int account_id { get; set; }
        public string user_name { get; set; }
        public DateTime event_date { get; set; }
        public string author_name { get; set; }
        public string category_name { get; set; }
        public string branch_name { get; set; }
    }
} 