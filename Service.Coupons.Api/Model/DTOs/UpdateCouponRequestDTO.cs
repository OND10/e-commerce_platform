namespace Service.Coupons.Api.Model.DTOs
{
    public class UpdateCouponRequestDTO
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public int MinAmount { get; set; }
        public string? StripeCouponId { get; set; }
    }
}
