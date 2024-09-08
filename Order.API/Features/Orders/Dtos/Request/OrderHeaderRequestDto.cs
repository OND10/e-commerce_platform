using Order.API.Entities;

namespace Order.API.Features.Orders.Dtos.Request
{
    public class OrderHeaderRequestDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double OrderTotal { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime OrderTime { get; set; }
        public string? Status { get; set; }
        //This properties for payment mechanism
        public string? PaymentIntentId { get; set; }
        public string? StripeSessionId { get; set; }
        //One to may relationship between the orderheader and orderdetails
        public IEnumerable<OrderDetailsRequestDto> OrderDetails { get; set; }
    }
}
