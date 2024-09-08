using eCommerceWebMVC.Models.DTOs.CartDTOs.Response;

namespace eCommerceWebMVC.Models.DTOs.CartDTOs.Request
{
    public class CartDto
    {
        public CartHeaderResponseDto? CartHeaderResponse { get; set; }
        public IEnumerable<CartDetailsResponseDto>? CartDetailsResponse { get; set; }
    }
}
