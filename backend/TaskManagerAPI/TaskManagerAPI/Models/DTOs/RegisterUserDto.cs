using System.ComponentModel.DataAnnotations;
using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.Models.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        [MinLength(6, ErrorMessage = "Username must be at least 6 characters long.")]
        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public required string Username { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public required string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        public required string Password { get; set; } = string.Empty;

    }
}
