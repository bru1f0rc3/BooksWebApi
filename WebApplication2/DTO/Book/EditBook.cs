namespace WebApplication2.DTO.Book
{
    public class EditBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Fragment { get; set; }
        public string CoverLink { get; set; }
        public int AuthorId { get; set; }
        public int BranchId { get; set; }
        public int CategoryId { get; set; }
    }
}
