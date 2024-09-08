using Order.API.Features.Dtos.Request;

namespace Order.API.Features.Orders.Services.Interface
{
    public interface IOrderService
    {
        Task<CartDto> ProcessOrder(CartDto cart);
    }
}
