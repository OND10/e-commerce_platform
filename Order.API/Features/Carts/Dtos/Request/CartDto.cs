
using Order.API.Features.Carts.Dtos.Response;

namespace Order.API.Features.Dtos.Request
{
    public class CartDto
    {
        public CartHeaderResponseDto? CartHeaderResponse { get; set; }
        public IEnumerable<CartDetailsResponseDto>? CartDetailsResponse { get; set; }
    }
}
