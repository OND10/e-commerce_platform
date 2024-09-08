namespace Email.API.Features.DTOs.CartHeaderDTOs.Request
{
    public class CartHeaderRequestDto
    {
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double CartTotal { get; set; }
    }
}
