namespace Auth.API.Models.DTOs.Response
{
    public class LoginResponseDTO
    {
        private UserDTO user { get; set; }
        public string Token { get; set; } = null!;


        public UserDTO User
        {
            get
            {
                return new UserDTO
                {
                    Email = user.Email,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                };
            }
            set
            {
                user = value;
            }
        }
    }
}
