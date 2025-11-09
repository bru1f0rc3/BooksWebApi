namespace WebApplication2.DTO.Category
{
    public class CategoryDTO
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class CreateCategoryDTO
    {
        public string name { get; set; }
    }

    public class UpdateCategoryDTO
    {
        public string name { get; set; }
    }
} 