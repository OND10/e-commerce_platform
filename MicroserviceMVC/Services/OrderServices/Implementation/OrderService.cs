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
                return Result<OrderHeaderResponseDto>.Success(data, "Created Successfully", true);
            }
            else
            {
                return Result<OrderHeaderResponseDto>.Faild(false, result.Message);
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
                return Result<StripeRequestDto>.Success(data, "Created Successfully", true);
            }
            else
            {
                return Result<StripeRequestDto>.Faild(false, result.Message);
            }
        }

        public async Task<Result<IEnumerable<OrderHeaderResponseDto>>> GetAllOrders(string? userId)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = HttpMethodType.ApiType.Get,
                Url = $"{HttpMethodType.OrderAPIBase}/api/order/GetOrders?{userId}",
            });

            if (result.IsSuccess)
            {
                if (result.Response.Data is not null)
                {
                    var data = JsonConvert.DeserializeObject<IEnumerable<OrderHeaderResponseDto>>(result.Response.Data.ToString());
                    return Result<IEnumerable<OrderHeaderResponseDto>>.Success(data, "Get All Orders Successfully", true);
                }
                else
                {
                    return Result<IEnumerable<OrderHeaderResponseDto>>.Faild(false, "result.Response.Data is null");
                }
            }
            else
            {
                return Result<IEnumerable<OrderHeaderResponseDto>>.Faild(false, result.Message);
            }
        }

        public async Task<Result<OrderHeaderResponseDto>> GetOrderById(int orderId)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = HttpMethodType.ApiType.Get,
                Url = $"{HttpMethodType.OrderAPIBase}/api/order/GetOrder/{orderId}",
            });

            if (result.IsSuccess)
            {
                if (result.Response.Data is not null)
                {
                    var data = JsonConvert.DeserializeObject<OrderHeaderResponseDto>(result.Response.Data.ToString());
                    return  Result<OrderHeaderResponseDto>.Success(data, "Order is Found Successfully", true);
                }
                else
                {
                    return Result<OrderHeaderResponseDto>.Faild(false, "result.Response.Data is null");
                }
            }
            else
            {
                return Result<OrderHeaderResponseDto>.Faild(false, result.Message);
            }
        }

        public async Task<Result<bool>> UpdateOrderStatus(int orderId, string newStatus)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = HttpMethodType.ApiType.Post,
                Url = $"{HttpMethodType.OrderAPIBase}/api/order/UpdateOrderStatus/{orderId}",
                Data = newStatus
            });

            if (result.IsSuccess)
            {
                var responseData = result.Response.Data.ToString();
                if (bool.TryParse(responseData, out var data))
                {
                    return  Result<bool>.Success(data, "Order Status is updated Successfully", true);
                }
                else
                {
                    // Log or handle unexpected content
                    Console.WriteLine("Unexpected response data format.");
                    return  Result<bool>.Faild(false, "Unexpected response data format.");
                }
            }

            return Result<bool>.Faild(false, result.Message);
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
                return Result<OrderHeaderResponseDto>.Success(data, "Order is Verified Successfully", true);
            }

            else
            {
                return Result<OrderHeaderResponseDto>.Faild(false, result.Message);
            }
        }
    }
}
