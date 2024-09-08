using Order.API.Features.Dtos.Request;
using Order.API.Features.Orders.Services.Interface;

namespace Order.API.Features.Orders.Services.Implementation
{
    public class ValidationDecorator : OrderServiceDecorator
    {
        public ValidationDecorator(IOrderService orderService):base(orderService) { }

        public override Task<CartDto> ProcessOrder(CartDto cart)
        {

            if(cart.CartHeaderResponse.Discount > 0 && !string.IsNullOrEmpty(cart.CartHeaderResponse.Name))
            {
                cart.CartHeaderResponse.isValid = true;
            }
            else
            {
                cart.CartHeaderResponse.isValid = false;
            }
            return base.ProcessOrder(cart);
        }

    }
}
