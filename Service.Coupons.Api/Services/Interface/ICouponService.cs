using Service.Coupons.Api.Common.Handler;
using Service.Coupons.Api.Model.DTOs;

namespace Service.Coupons.Api.Services.Interface
{
    public interface ICouponService
    {
        Task<Result<CouponResponseDTO>> CreaAsync(CouponRequestDTO model);
        Task<Result<IEnumerable<CouponResponseDTO>>> GetAllAsync();
        Task<Result<CouponResponseDTO>> GetByIdAsync(int id);
        Task<Result<CouponResponseDTO>> GetByCodeAsync(string code);
        Task<Result<CouponResponseDTO>> UpdateAsync(UpdateCouponRequestDTO model);
        Task<Result<bool>> DeleteAsync(int id);

    }
}
