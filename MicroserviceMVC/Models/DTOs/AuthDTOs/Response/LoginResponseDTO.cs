namespace eCommerceWebMVC.Models.DTOs.AuthDTOs.Response
{
    public class LoginResponseDTO
    {
        public UserDTO user { get; set; }
        public string Token { get; set; } = null!;
    }
}
