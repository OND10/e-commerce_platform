using eCommerceWebMVC.Models.DTOs.CartDTOs.Request;
using eCommerceWebMVC.Services.CartServices.Interface;
using MicroserviceMVC.Common.Enum;
using MicroserviceMVC.Common.Handler;
using MicroserviceMVC.Service.WebServices.Interface;
using Newtonsoft.Json;
using static MicroserviceMVC.Common.Enum.HttpMethodType;

namespace eCommerceWebMVC.Services.CartServices.Implementation
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public CartService()
        {

        }

        public async Task<Result<bool>> ApplyCouponAsync(CartDto cart)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = ApiType.Post,
                Url = $"{HttpMethodType.CartAPIBase}/api/cart/ApplyCoupon",
                Data = cart
            });

            if (result.IsSuccess)
            {
                var responseData = result.Response.Data.ToString();
                if (bool.TryParse(responseData, out var data))
                {
                    return await Result<bool>.SuccessAsync(data, "Coupon is applied to Cart Successfully", true);
                }
                else
                {
                    // Log or handle unexpected content
                    Console.WriteLine("Unexpected response data format.");
                    return await Result<bool>.FaildAsync(false, "Unexpected response data format.");
                }
            }
            return await Result<bool>.FaildAsync(false, result.Message);

        }

        public async Task<Result<bool>> DeleteAsync(int cartDetailsId)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = HttpMethodType.ApiType.Post,
                Url = $"{HttpMethodType.CartAPIBase}/api/cart/removeCart",
                Data = cartDetailsId
            });
            if (result.IsSuccess)
            {
                //var data = JsonConvert.DeserializeObject<bool>(result.Response.IsSuccess.ToString());
                var responseData = result.Response.Data.ToString();
                if (bool.TryParse(responseData, out var data))
                {
                    return await Result<bool>.SuccessAsync(data, "Deleted Successfully", true);
                }
                else
                {
                    // Log or handle unexpected content
                    Console.WriteLine("Unexpected response data format.");
                    return await Result<bool>.FaildAsync(false, "Unexpected response data format.");
                }

            }
            return await Result<bool>.FaildAsync(false, result.Message);
        }

        public async Task<Result<IEnumerable<CartDto>>> GetAsync(string userId)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = HttpMethodType.ApiType.Get,
                Url = $"{HttpMethodType.CartAPIBase}/api/cart/" + userId
            }, withBearer: true);

            if (result.IsSuccess)
            {
                if (result.Response.Data is not null)
                {
                    var data = JsonConvert.DeserializeObject<IEnumerable<CartDto>>(result.Response.Data.ToString());
                    return await Result<IEnumerable<CartDto>>.SuccessAsync(data, "Viewed Successfully", true);
                }
                else
                {
                    return await Result<IEnumerable<CartDto>>.FaildAsync(false, "result.Response.Data is null");
                }
            }
            else
            {
                return await Result<IEnumerable<CartDto>>.FaildAsync(false, result.Message);
            }
        }

        public async Task<Result<CartDto>> CreateAsync(CartDto cartDto)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = ApiType.Post,
                Url = $"{HttpMethodType.CartAPIBase}/api/cart/createCart",
                Data = cartDto
            });

            if (result.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<CartDto>(result.Response.Data.ToString());
                return await Result<CartDto>.SuccessAsync(data, "Created Successfully", true);
            }
            else
            {
                return await Result<CartDto>.FaildAsync(false, result.Message);
            }
        }

        public async Task<Result<bool>> RemoveCouponAsync(CartDto cart)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = ApiType.Post,
                Url = $"{HttpMethodType.CartAPIBase}/api/cart/RemoveCoupon",
                Data = cart
            });

            if (result.IsSuccess)
            {
                var responseData = result.Response.Data.ToString();
                if (bool.TryParse(responseData, out var data))
                {
                    return await Result<bool>.SuccessAsync(data, "Coupon Removed from Cart Successfully", true);
                }
                else
                {
                    // Log or handle unexpected content
                    Console.WriteLine("Unexpected response data format.");
                    return await Result<bool>.FaildAsync(false, "Unexpected response data format.");
                }
            }
            return await Result<bool>.FaildAsync(false, result.Message);
        }

        public async Task<Result<bool>> EmailCart(CartDto cart)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = ApiType.Post,
                Url = $"{HttpMethodType.CartAPIBase}/api/cart/EmailCartRequest",
                Data = cart
            });

            if (result.IsSuccess)
            {
                var responseData = result.Response.Data.ToString();
                if (bool.TryParse(responseData, out var data))
                {
                    return await Result<bool>.SuccessAsync(data, "Coupon is applied to Cart Successfully", true);
                }
                else
                {
                    // Log or handle unexpected content
                    Console.WriteLine("Unexpected response data format.");
                    return await Result<bool>.FaildAsync(false, "Unexpected response data format.");
                }
            }
            return await Result<bool>.FaildAsync(false, result.Message);
        }
    }
}
