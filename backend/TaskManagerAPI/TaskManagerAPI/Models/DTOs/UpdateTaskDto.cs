using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models.DTOs
{
    public class UpdateTaskDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters long.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public required string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public required string Description { get; set; } = string.Empty;
        [Required]
        public DateTime? DueDate { get; set; }
        [Required]
        public bool IsCompleted { get; set; } = false;
    }
}
