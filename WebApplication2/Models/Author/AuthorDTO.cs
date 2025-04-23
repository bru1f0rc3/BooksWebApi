namespace WebApplication2.DTO.Author
{
    public class AuthorDTO
    {
        public int id { get; set; }
        public string full_name { get; set; }
    }

    public class CreateAuthorDTO
    {
        public string full_name { get; set; }
    }

    public class UpdateAuthorDTO
    {
        public string full_name { get; set; }
    }
} 