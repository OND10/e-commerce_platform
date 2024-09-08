using System.ComponentModel.DataAnnotations;

namespace eCommerceWebMVC.Models.DTOs.AuthDTOs.Request
{
    public class RegisterRequestDTO
    {

        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
