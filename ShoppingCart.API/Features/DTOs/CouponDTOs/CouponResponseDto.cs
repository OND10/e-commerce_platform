namespace ShoppingCart.API.Features.DTOs.CouponDTOs
{
    public class CouponResponseDto
    {
        public int CouponId { get; set; }

        public string CouponCode { get; set; } = string.Empty;

        public decimal DiscountAmount { get; set; }

        public int MinAmount { get; set; }
    }
}
