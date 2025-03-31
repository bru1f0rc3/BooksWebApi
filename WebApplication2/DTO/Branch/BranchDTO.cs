namespace WebApplication2.DTO.Branch
{
    public class BranchDTO
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class CreateBranchDTO
    {
        public string name { get; set; }
    }

    public class UpdateBranchDTO
    {
        public string name { get; set; }
    }
} 