namespace WebApplication2.DTO.Dashboard
{
    public class ChangePasswordDTO
    {
        public int id { get; set; }
        public string old_password { get; set; }
        public string new_password { get; set; }
        public string email { get; set; }
        public string code { get; set; }
    }
}
