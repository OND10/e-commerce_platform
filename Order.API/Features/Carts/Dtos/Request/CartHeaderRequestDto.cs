namespace Order.API.Features.Carts.Dtos.Request
{
    public class CartHeaderRequestDto
    {
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double CartTotal { get; set; }
    }
}
