using Order.API.Features.Orders.Dtos.Response;

namespace Order.API.Features.Stripe.Dtos.Request
{
    public class StripeRequestDto
    {
        public string? StripeSessionUrl { get; set; }
        public string? StripeSessionId { get; set; }
        public string? ApprovedUrl { get; set; }
        public string? CancelUrl { get; set; }
        public OrderHeaderResponseDto OrderHeader { get; set; }
    }
}
