using System.ComponentModel.DataAnnotations;

namespace FootballSimulator.Application.Models
{
    public class UserEditModel : EditModelBase
    {
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "User Name is required.")]
        [MaxLength(100, ErrorMessage = "User Name cannot exceed 100 characters.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [MaxLength(256, ErrorMessage = "Email cannot exceed 256 characters.")]
        public string? Email { get; set; }
        public string? ApplicationUserId { get; set; }
    }
}
