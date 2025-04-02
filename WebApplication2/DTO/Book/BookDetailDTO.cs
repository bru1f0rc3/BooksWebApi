namespace WebApplication2.DTO.Book
{
    public class BookDetailDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string fragment { get; set; }
        public string cover_link { get; set; }
        public int author_id { get; set; }
        public string author_name { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public int branch_id { get; set; }
        public string branch_name { get; set; }
    }
} 