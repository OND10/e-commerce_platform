using Newtonsoft.Json;
using ShoppingCart.API.Common.Enum;
using ShoppingCart.API.Common.Handler;
using ShoppingCart.API.Features.DTOs.CouponDTOs;

namespace ShoppingCart.API.Features.Coupons
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<Result<CouponResponseDto>> GetCoupon(string code)
        {
            HttpClient client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"{HttpMethodType.CouponAPIBase}/api/coupon/{code}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<Shared.HttpResponse>(apiContent);
            if (resp != null && resp.IsSuccess)
            {
                var obj = JsonConvert.DeserializeObject<CouponResponseDto>(Convert.ToString(resp.Data));
                return await Result<CouponResponseDto>.SuccessAsync(obj, "Viewed Successfully", true);
            }

            return await Result<CouponResponseDto>.FaildAsync(false, "Not Viewed");
        }

    }
}
