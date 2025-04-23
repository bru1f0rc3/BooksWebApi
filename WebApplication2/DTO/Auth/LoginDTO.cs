namespace WebApplication2.DTO.Auth
{
    public class LoginDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class TokenDTO 
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
    }
} 