using System.ComponentModel.DataAnnotations;

namespace eCommerceWebMVC.Models.DTOs.AuthDTOs.Request
{
    public class LoginRequestDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
