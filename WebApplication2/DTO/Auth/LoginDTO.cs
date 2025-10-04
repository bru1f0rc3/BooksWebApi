using System.ComponentModel.DataAnnotations;

namespace WebApplication2.DTO.Auth
{
    public class LoginDTO
    {
        [Required]
        public required string Login { get; set; }
        
        [Required]
        public required string Password { get; set; }
    }
    
    public class TokenDTO 
    {
        [Required]
        public required string Token { get; set; }
        
        [Required]
        public required string Role { get; set; }
        
        public int UserId { get; set; }
        
        [Required]
        public required string FullName { get; set; }
    }
}
