using System.ComponentModel.DataAnnotations;
using MicroserviceMVC.Models;

namespace eCommerceWebMVC.Models.DTOs.CouponDTOs.Request
{
    public class CouponRequestDTO
    {
        //public int CouponId { get; set; }

        public string CouponCode { get; set; } = string.Empty;

        public decimal DiscountAmount { get; set; }

        public int MinAmount { get; set; }

        public async Task<CouponRequestDTO> ToRequest(Coupon model)
        {
            return await Task.FromResult(new CouponRequestDTO
            {
                //CouponId = model.CouponId,
                CouponCode = model.CouponCode,
                DiscountAmount = model.DiscountAmount,
                MinAmount = model.MinAmount,
            });
        }

    }
}
