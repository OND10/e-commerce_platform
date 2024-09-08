using eCommerceWebMVC.Models.DTOs.CartDTOs.Request;
using eCommerceWebMVC.Models.DTOs.OrderDTOs;
using eCommerceWebMVC.Models.DTOs.StripeDTOs.Request;
using eCommerceWebMVC.Services.OrderServices.Interface;
using MicroserviceMVC.Common.Enum;
using MicroserviceMVC.Common.Handler;
using MicroserviceMVC.Service.WebServices.Interface;
using Newtonsoft.Json;
using static MicroserviceMVC.Common.Enum.HttpMethodType;

namespace eCommerceWebMVC.Services.OrderServices.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;
        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<Result<OrderHeaderResponseDto>> CreatAsync(CartDto model)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = ApiType.Post,
                Url = $"{HttpMethodType.OrderAPIBase}/api/order/CreateOrder",
                Data = model
            });

            if (result.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<OrderHeaderResponseDto>(result.Response.Data.ToString());
                return await Result<OrderHeaderResponseDto>.SuccessAsync(data, "Created Successfully", true);
            }
            else
            {
                return await Result<OrderHeaderResponseDto>.FaildAsync(false, result.Message);
            }
        }

        public async Task<Result<StripeRequestDto>> CreateStripeSession(StripeRequestDto model)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = ApiType.Post,
                Url = $"{HttpMethodType.OrderAPIBase}/api/order/CreateStripeSession",
                Data = model
            });

            if (result.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<StripeRequestDto>(result.Response.Data.ToString());
                return await Result<StripeRequestDto>.SuccessAsync(data, "Created Successfully", true);
            }
            else
            {
                return await Result<StripeRequestDto>.FaildAsync(false, result.Message);
            }
        }

        public async Task<Result<OrderHeaderResponseDto>> VerifyStripeSession(int orderHeaderId)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = ApiType.Post,
                Url = $"{HttpMethodType.OrderAPIBase}/api/order/ValidateStripeSession",
                Data = orderHeaderId
            });

            if(result.IsSuccess && result.Response.Data is not null)
            {
                var data = JsonConvert.DeserializeObject<OrderHeaderResponseDto>(result.Response.Data.ToString());
                return await Result<OrderHeaderResponseDto>.SuccessAsync(data, "Order is Verified Successfully", true);
            }

            else
            {
                return await Result<OrderHeaderResponseDto>.FaildAsync(false, result.Message);
            }
        }
    }
}
