using System.ComponentModel.DataAnnotations;

namespace eCommerceWebMVC.Models.DTOs.CouponDTOs.Response
{
    public class CouponResponseDTO
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
