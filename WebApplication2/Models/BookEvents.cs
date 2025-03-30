namespace WebApplication2.Models
{
    public class BookEvents
    {
        public int id { get; set; }
        public int account_id { get; set; }
        public int book_id { get; set; }
        public int event_type_id { get; set; }
        public DateTime event_date { get; set; }
    }
}
