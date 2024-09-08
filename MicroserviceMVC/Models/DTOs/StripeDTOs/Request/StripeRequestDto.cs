using eCommerceWebMVC.Models.DTOs.OrderDTOs;

namespace eCommerceWebMVC.Models.DTOs.StripeDTOs.Request
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
