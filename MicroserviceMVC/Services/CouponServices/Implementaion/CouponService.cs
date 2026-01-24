using eCommerceWebMVC.Models.DTOs.CouponDTOs.Request;
using eCommerceWebMVC.Models.DTOs.CouponDTOs.Response;
using MicroserviceMVC.Common.Enum;
using MicroserviceMVC.Common.Handler;
using MicroserviceMVC.Service.CouponServices.Interface;
using MicroserviceMVC.Service.WebServices.Interface;
using Newtonsoft.Json;
using static MicroserviceMVC.Common.Enum.HttpMethodType;

namespace MicroserviceMVC.Service.CouponServices.Implementaion
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<Result<CouponResponseDTO>> CreaAsync(CouponRequestDTO model)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = ApiType.Post,
                Url = $"{HttpMethodType.CouponAPIBase}/api/coupon/",
                Data = model
            });

            if (result.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<CouponResponseDTO>(result.Response.Data.ToString());
                return Result<CouponResponseDTO>.Success(data, "Created Successfully", true);
            }
            else
            {
                return Result<CouponResponseDTO>.Faild(false, result.Message);
            }
            //return await Result<CouponResponseDTO>.SuccessAsync(, "Added Successfully", true);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = HttpMethodType.ApiType.Delete,
                Url = $"{HttpMethodType.CouponAPIBase}/api/coupon/{id}"
            });
            if (result.IsSuccess)
            {
                //var data = JsonConvert.DeserializeObject<bool>(result.Response.IsSuccess.ToString());
                var responseData = result.Response.Data.ToString();
                if (bool.TryParse(responseData, out var data))
                {
                    return Result<bool>.Success(data, "Deleted Successfully", true);
                }
                else
                {
                    // Log or handle unexpected content
                    Console.WriteLine("Unexpected response data format.");
                    return Result<bool>.Faild(false, "Unexpected response data format.");
                }

            }
            return Result<bool>.Faild(false, result.Message);
        }

        public async Task<Result<IEnumerable<CouponResponseDTO>>> GetAllAsync()
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = HttpMethodType.ApiType.Get,
                Url = $"{HttpMethodType.CouponAPIBase}/api/coupon/"
            },withBearer:true);

            if (result.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<IEnumerable<CouponResponseDTO>>(result.Response.Data.ToString());
                return Result<IEnumerable<CouponResponseDTO>>.Success(data, "Viewed Successfully", true);
            }
            else
            {
                return Result<IEnumerable<CouponResponseDTO>>.Faild(false, result.Message);
            }
        }

        public async Task<Result<CouponResponseDTO>> GetByCodeAsync(string code)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = HttpMethodType.ApiType.Get,
                Url = $"{HttpMethodType.CouponAPIBase}/api/coupon/{code}"
            });
            if (result.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<CouponResponseDTO>(result.Response.Data.ToString());
                return Result<CouponResponseDTO>.Success(data, "Found Successfully", true);
            }

            return Result<CouponResponseDTO>.Faild(false, result.Message);
        }

        public async Task<Result<CouponResponseDTO>> GetByIdAsync(int id)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = HttpMethodType.ApiType.Get,
                Url = $"{HttpMethodType.CouponAPIBase}/api/coupon/{id}"
            });
            if (result.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<CouponResponseDTO>(result.Response.Data.ToString());
                return Result<CouponResponseDTO>.Success(data, "Found Successfully", true);
            }

            return Result<CouponResponseDTO>.Faild(false, result.Message);
        }

        public async Task<Result<CouponResponseDTO>> UpdateAsync(int id, CouponRequestDTO model)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = ApiType.Put,
                Url = $"{HttpMethodType.CouponAPIBase}/api/coupon/{id}",
                Data = model
            });

            if (result.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<CouponResponseDTO>(result.Response.Data.ToString());
                return Result<CouponResponseDTO>.Success(data, "Updated Successfully", true);
            }
            else
            {
                return Result<CouponResponseDTO>.Faild(false, result.Message);
            }
        }
    }
}
