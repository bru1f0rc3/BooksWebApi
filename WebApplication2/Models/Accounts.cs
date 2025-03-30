namespace WebApplication2.Models
{
    public class Accounts
    {
        public int id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string full_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public int role_id { get; set; }
        public DateTime created_at { get; set; }
    }
} 