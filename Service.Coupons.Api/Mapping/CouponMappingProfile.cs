using AutoMapper;
using Service.Coupons.Api.Model;
using Service.Coupons.Api.Model.DTOs;

namespace Service.Coupons.Api.Mapping
{
    public class CouponMappingProfile:Profile
    {
        public CouponMappingProfile()
        {
            CreateMap<CouponRequestDTO, Service.Coupons.Api.Model.Coupon>();
            CreateMap<Service.Coupons.Api.Model.Coupon, CouponRequestDTO>();
            CreateMap<Service.Coupons.Api.Model.Coupon, CouponResponseDTO>();
        }
    }
}
