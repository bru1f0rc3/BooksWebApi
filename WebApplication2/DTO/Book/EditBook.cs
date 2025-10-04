using System.ComponentModel.DataAnnotations;

namespace WebApplication2.DTO.Book
{
    public class EditBook
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 2000 characters")]
        public string Description { get; set; }

        [StringLength(5000, ErrorMessage = "Fragment cannot exceed 5000 characters")]
        public string Fragment { get; set; }

        public string CoverLink { get; set; }

        [Required(ErrorMessage = "AuthorId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "AuthorId must be greater than 0")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "BranchId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "BranchId must be greater than 0")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0")]
        public int CategoryId { get; set; }
    }
}
