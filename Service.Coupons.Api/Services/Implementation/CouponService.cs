using AutoMapper;
using OnMapper;
using Service.Coupons.Api.Common.Handler;
using Service.Coupons.Api.Contracts.Interfaces;
using Service.Coupons.Api.Model;
using Service.Coupons.Api.Model.DTOs;
using Service.Coupons.Api.Services.Interface;

namespace Service.Coupons.Api.Services.Implementation
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository<Service.Coupons.Api.Model.Coupon> _repo;
        private readonly IMapper _mapper;
        public CouponService(ICouponRepository<Service.Coupons.Api.Model.Coupon> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<CouponResponseDTO>> CreaAsync(CouponRequestDTO model)
        {

            var objectmodel = _mapper.Map<Service.Coupons.Api.Model.Coupon>(model);
            var add = await _repo.CreateAsync(objectmodel);
            var requestobj = _mapper.Map<CouponResponseDTO>(add);
            return await Result<CouponResponseDTO>.SuccessAsync(requestobj, "Added successfully", true);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var delete = await _repo.DeleteAsync(id);
            var mapper = new OnMapping();
            var mappedResult = await mapper.Map<Service.Coupons.Api.Model.Coupon, bool>(delete);

            return await Result<bool>.SuccessAsync(mappedResult.Data, "Deleted Successfully", true);        
        }

        //public IResponse DeleteAsync(CouponRequestDTO model)
        //{
        //    
        //}

        public async Task<Result<IEnumerable<CouponResponseDTO>>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            var responselist = new CouponResponseDTO();
            var convert = responselist.FromModel(list);
            return await Result<IEnumerable<CouponResponseDTO>>.SuccessAsync(convert, "Viewd successfully");
        }

        public async Task<Result<CouponResponseDTO>> GetByCodeAsync(string code)
        {
            var findcode = await _repo.GetByCodeAsync(code);
            var mapper = new OnMapping();
            var mappedResult = await mapper.Map<Service.Coupons.Api.Model.Coupon, CouponResponseDTO>(findcode);
            return await Result<CouponResponseDTO>.SuccessAsync(mappedResult.Data, "Found successfully");
        }

        //public async Task<IResponse<CouponRequestDTO>> GetByCodeAsync(string code)
        //{
        //    var findcode = await _repo.GetByCodeAsync(code);
        //    var requestcode = new CouponRequestDTO();

        //    return await Response<CouponRequestDTO>.SuccessAsync(await requestcode.ToRequest(findcode), "Found successfully");
        //}

        public async Task<Result<CouponResponseDTO>> GetByIdAsync(int id)
        {
            var findId = await _repo.GetByIdAsync(id);
            var mapper = new OnMapping();
            var mappedResult = await mapper.Map<Service.Coupons.Api.Model.Coupon, CouponResponseDTO>(findId);
            return await Result<CouponResponseDTO>.SuccessAsync(mappedResult.Data, "Found id Successfully ");

        }

        public async Task<Result<CouponResponseDTO>> UpdateAsync(UpdateCouponRequestDTO model)
        {
            var mapper = new OnMapping();
            var objectmodel = await mapper.Map<UpdateCouponRequestDTO, Service.Coupons.Api.Model.Coupon>(model);
            var add = await _repo.UpdateAsync(objectmodel.Data);
            var mappedResult = await mapper.Map<Service.Coupons.Api.Model.Coupon, CouponResponseDTO>(add);
            return await Result<CouponResponseDTO>.SuccessAsync(mappedResult.Data, "Added successfully");
        }


    }
}
