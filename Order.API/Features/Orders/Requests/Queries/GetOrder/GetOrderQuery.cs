using MediatR;
using SharedKernel.Results;
using Order.API.Features.Orders.Dtos.Response;
using System.Collections.Generic;

namespace Order.API.Features.Orders.Requests.Queries.GetOrder
{
    public class GetOrderQuery : IRequest<Result<IEnumerable<OrderHeaderResponseDto>>>
    {
        public string Role { get; set; }
        public string userId { get; set; }
    }
}
