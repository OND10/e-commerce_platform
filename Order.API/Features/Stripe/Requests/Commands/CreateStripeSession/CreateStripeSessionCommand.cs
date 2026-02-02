using MediatR;
using SharedKernels.Results;
using Order.API.Features.Stripe.Dtos.Request;
using Order.API.Features.Orders.Dtos.Response;
using SharedKernels.Abstractions.Messaging;

namespace Order.API.Features.Stripe.Requests.Commands.CreateStripeSession
{
    public class CreateStripeSessionCommand : ICommand<StripeRequestDto>
    {
        public string? StripeSessionUrl { get; set; }
        public string? StripeSessionId { get; set; }
        public string ApprovedUrl { get; set; }
        public string CancelUrl { get; set; }
        public OrderHeaderResponseDto OrderHeader { get; set; }
    }
}
