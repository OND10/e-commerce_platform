using MediatR;
using ShoppingCart.API.Common.Handler;
using ShoppingCart.API.Features.DTOs.CartDetailsDTOs.Response;
using ShoppingCart.API.Features.DTOs.CartHeaderDTOs.Response;

namespace ShoppingCart.API.Features.Carts.Requests.Command.RemoveCartCoupon
{
    public class RemoveCartCouponCommand : IRequest<Result<bool>>
    {
        public CartHeaderResponseDto CartHeaderResponse { get; set; }
        public IEnumerable<CartDetailsResponseDto> CartDetailsResponse { get; set; }
    }
}
