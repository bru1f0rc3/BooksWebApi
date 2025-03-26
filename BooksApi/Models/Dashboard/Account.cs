using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BooksApi.Models.Dashboard
{
    [Table("Accounts")]
    public class Account : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("role_id")]
        public int Role { get; set; }
    }
}
