using MediatR;
using SharedKernels.Results;
using Order.API.Features.Orders.Dtos.Response;
using SharedKernels.Abstractions.Messaging;

namespace Order.API.Features.Orders.Requests.Queries.GetOrderById
{
    public class GetOrderQueryById : IQuery<OrderHeaderResponseDto>
    {
        public int orderId { get; set; }
    }
}
