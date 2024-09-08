using MediatR;
using ShoppingCart.API.Common.Handler;
using ShoppingCart.API.Features.DTOs.CartDetailsDTOs.Response;
using ShoppingCart.API.Features.DTOs.CartDTOs;
using ShoppingCart.API.Features.DTOs.CartHeaderDTOs.Response;

namespace ShoppingCart.API.Features.Carts.Requests.Command.AddCart
{
    public class AddCartCommand : IRequest<Result<CartDto>>
    {
        public CartHeaderResponseDto CartHeaderResponse { get; set; }
        public IEnumerable<CartDetailsResponseDto> CartDetailsResponse { get; set; }
    }
}
