using MediatR;
using Order.API.Common.Handler;
using Order.API.Features.Orders.Dtos.Response;
using Order.API.Features.Stripe.Dtos.Request;
using Order.API.Features.Stripe.Dtos.Response;

namespace Order.API.Features.Stripe.Requests.Queries.ValidateStripeSession
{
    public class ValidateStripeSessionQuery : IRequest<Result<OrderHeaderResponseDto>>
    {
        public int OrderHeadreId {  get; set; }
    }
}
