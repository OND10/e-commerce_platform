using MediatR;
using ShoppingCart.API.Common.Handler;

namespace ShoppingCart.API.Features.Carts.Requests.Command.DeleteCart
{
    public class DeleteCartCommand : IRequest<Result<bool>>
    {
        public int cartDetailsId {  get; set; }
    }
}
