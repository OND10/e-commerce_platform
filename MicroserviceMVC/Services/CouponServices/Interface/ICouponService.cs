using eCommerceWebMVC.Models.DTOs.CouponDTOs.Request;
using eCommerceWebMVC.Models.DTOs.CouponDTOs.Response;
using MicroserviceMVC.Common.Handler;

namespace MicroserviceMVC.Service.CouponServices.Interface
{
    public interface ICouponService
    {
        Task<Result<CouponResponseDTO>> CreaAsync(CouponRequestDTO model);
        Task<Result<IEnumerable<CouponResponseDTO>>> GetAllAsync();
        Task<Result<CouponResponseDTO>> GetByIdAsync(int id);
        Task<Result<CouponResponseDTO>> GetByCodeAsync(string code);
        Task<Result<CouponResponseDTO>> UpdateAsync(int id, CouponRequestDTO model);
        Task<Result<bool>> DeleteAsync(int id);

    }
}
