using ShoppingCart.API.Features.DTOs.CartDetailsDTOs.Request;
using ShoppingCart.API.Features.DTOs.CartDetailsDTOs.Response;
using ShoppingCart.API.Features.DTOs.CartHeaderDTOs.Request;
using ShoppingCart.API.Features.DTOs.CartHeaderDTOs.Response;

namespace ShoppingCart.API.Features.DTOs.CartDTOs
{
    public class CartDto
    {
        public CartHeaderResponseDto? CartHeaderResponse { get; set; }
        public IEnumerable<CartDetailsResponseDto>? CartDetailsResponse { get; set; }
    }
}
