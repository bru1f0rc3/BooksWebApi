namespace BooksApi.DTO.Dashboard
{
    public class AccountsDTO
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
    }
}
