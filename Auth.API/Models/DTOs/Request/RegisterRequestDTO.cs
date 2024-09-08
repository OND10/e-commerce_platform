namespace Auth.API.Models.DTOs.Request
{
    public class RegisterRequestDTO
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = string.Empty;

    }
}
