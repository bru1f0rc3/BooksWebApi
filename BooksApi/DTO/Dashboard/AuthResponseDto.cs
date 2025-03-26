namespace BooksApi.DTO.Dashboard
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public AccountsDTO User { get; set; }
        public string Message { get; set; }
    }
} 