using MediatR;
using ShoppingCart.API.Common.Handler;
using ShoppingCart.API.Features.DTOs.CartDTOs;

namespace ShoppingCart.API.Features.Carts.Requests.Queries.GetCarts
{
    public class GetCartsQuery : IRequest<Result<IEnumerable<CartDto>>>
    {
        public string UserId {  get; set; }
    }
}
