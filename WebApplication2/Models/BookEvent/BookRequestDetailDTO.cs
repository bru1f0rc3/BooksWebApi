namespace WebApplication2.DTO.BookEvent
{
    public class BookRequestDetailDTO
    {
        public int event_id { get; set; }
        public int book_id { get; set; }
        public string book_title { get; set; }
        public string book_description { get; set; }
        public string category_name { get; set; }
        public string branch_name { get; set; }
        public int user_id { get; set; }
        public string user_fullname { get; set; }
        public string user_phone { get; set; }
        public DateTime event_date { get; set; }
    }
} 