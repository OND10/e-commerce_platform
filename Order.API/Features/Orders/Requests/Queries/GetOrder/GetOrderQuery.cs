using MediatR;
using SharedKernels.Results;
using Order.API.Features.Orders.Dtos.Response;
using System.Collections.Generic;
using SharedKernels.Abstractions.Messaging;

namespace Order.API.Features.Orders.Requests.Queries.GetOrder
{
    public class GetOrderQuery : IQuery<IEnumerable<OrderHeaderResponseDto>>
    {
        public string Role { get; set; }
        public string userId { get; set; }
    }
}
