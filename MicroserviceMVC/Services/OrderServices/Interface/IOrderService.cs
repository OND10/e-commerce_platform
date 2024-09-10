using eCommerceWebMVC.Models.DTOs.CartDTOs.Request;
using eCommerceWebMVC.Models.DTOs.OrderDTOs;
using eCommerceWebMVC.Models.DTOs.StripeDTOs.Request;
using MicroserviceMVC.Common.Handler;

namespace eCommerceWebMVC.Services.OrderServices.Interface
{
    public interface IOrderService
    {
        Task<Result<OrderHeaderResponseDto>> CreatAsync(CartDto model);
        Task<Result<StripeRequestDto>> CreateStripeSession(StripeRequestDto model);
        Task<Result<OrderHeaderResponseDto>> VerifyStripeSession(int orderHeaderId);
        Task<Result<bool>> UpdateOrderStatus(int orderId, string newStatus);
        Task<Result<IEnumerable<OrderHeaderResponseDto>>> GetAllOrders(string? userId);
        Task<Result<OrderHeaderResponseDto>> GetOrderById(int orderId);

    }
}
