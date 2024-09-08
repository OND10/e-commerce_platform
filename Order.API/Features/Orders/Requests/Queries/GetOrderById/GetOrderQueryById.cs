using MediatR;
using Order.API.Common.Handler;
using Order.API.Features.Orders.Dtos.Response;

namespace Order.API.Features.Orders.Requests.Queries.GetOrderById
{
    public class GetOrderQueryById : IRequest<Result<OrderHeaderResponseDto>>
    {
        public int orderId { get; set; }
    }
}
