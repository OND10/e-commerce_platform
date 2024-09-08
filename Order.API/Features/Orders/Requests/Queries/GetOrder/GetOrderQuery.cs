using MediatR;
using Order.API.Common.Handler;
using Order.API.Features.Orders.Dtos.Response;

namespace Order.API.Features.Orders.Requests.Queries.GetOrder
{
    public class GetOrderQuery : IRequest<Result<IEnumerable<OrderHeaderResponseDto>>>
    {
        public string Role {  get; set; }
        public string userId {  get; set; }
    }
}
