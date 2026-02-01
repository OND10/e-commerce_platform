using MediatR;
using SharedKernels.Results;
using Order.API.Features.Orders.Dtos.Response;

namespace Order.API.Features.Stripe.Requests.Queries.ValidateStripeSession
{
    public record ValidateStripeSessionQuery(int OrderHeadreId) : IRequest<Result<OrderHeaderResponseDto>>;
}
