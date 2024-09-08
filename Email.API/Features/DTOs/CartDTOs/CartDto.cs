using Email.API.Features.DTOs.CartDetailsDTOs.Response;
using Email.API.Features.DTOs.CartHeaderDTOs.Response;

namespace Email.API.Features.DTOs.CartDTOs
{
    public class CartDto
    {
        public CartHeaderResponseDto? CartHeaderResponse { get; set; }
        public IEnumerable<CartDetailsResponseDto>? CartDetailsResponse { get; set; }
    }
}
