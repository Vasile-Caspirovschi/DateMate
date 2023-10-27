using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string KnownAs { get; set; } = string.Empty;
        [Required]
        public string Gender { get; set; } = string.Empty;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
        [Required]
        [StringLength(8, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
