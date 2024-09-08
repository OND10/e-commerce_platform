using Order.API.Features.Dtos.Request;
using Order.API.Features.Orders.Services.Interface;

namespace Order.API.Features.Orders.Services.Implementation
{
    public abstract class OrderServiceDecorator : IOrderService
    {
        protected IOrderService _orderService;

        public OrderServiceDecorator(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public virtual Task<CartDto> ProcessOrder(CartDto cart)
        {
            return _orderService.ProcessOrder(cart);
        }
    }
}
