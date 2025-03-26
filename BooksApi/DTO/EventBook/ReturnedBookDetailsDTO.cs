namespace BooksApi.DTO.EventBook
{
    public class ReturnedBookDetailsDTO
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string BookTitle { get; set; }
        public string CoverLink { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 