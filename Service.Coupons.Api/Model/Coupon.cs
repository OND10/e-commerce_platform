﻿using System.ComponentModel.DataAnnotations;

namespace Service.Coupons.Api.Model
{
    public class Coupon
    {

        [Key]
        public int CouponId { get; set; }

        [Required(ErrorMessage = " The Coupon field is required")]
        [Display(Name = "CouponCode")]
        public string CouponCode { get; set; } = string.Empty;

        public decimal DiscountAmount { get; set; }

        public int MinAmount { get; set; }
        public string? StripeCouponId {  get; set; }
    }
}
