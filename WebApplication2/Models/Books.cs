namespace WebApplication2.Models
{
    public class Books
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string fragment { get; set; }
        public string cover_link { get; set; }
        public int author_id { get; set; }
        public int branch_id { get; set; }
        public int category_id { get; set; }
    }
}
