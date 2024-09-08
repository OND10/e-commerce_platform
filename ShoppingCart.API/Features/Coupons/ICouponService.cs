using ShoppingCart.API.Common.Handler;
using ShoppingCart.API.Features.DTOs.CouponDTOs;

namespace ShoppingCart.API.Features.Coupons
{
    public interface ICouponService
    {
        Task<Result<CouponResponseDto>> GetCoupon(string code);
    }
}
