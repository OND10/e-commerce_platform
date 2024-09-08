using MediatR;
using Order.API.Common.Handler;
using Order.API.Features.Orders.Dtos.Response;

namespace Order.API.Features.Orders.Requests.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommand : IRequest<Result<bool>>
    {
        public int orderId {  get; set; }
        public string newStatus {  get; set; }
    }
}
