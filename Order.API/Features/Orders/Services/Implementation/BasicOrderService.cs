using Order.API.Features.Dtos.Request;
using Order.API.Features.Orders.Services.Interface;

namespace Order.API.Features.Orders.Services.Implementation
{
    public class BasicOrderService : IOrderService
    {
        public async Task<CartDto> ProcessOrder(CartDto cart)
        {
            // Basic order processing logic
            return await Task.FromResult<CartDto>(cart);
        }
    }
}
