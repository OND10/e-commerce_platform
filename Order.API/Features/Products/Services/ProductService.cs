using Newtonsoft.Json;
using Order.API.Features.Products.Dtos.Response;
using Order.API.Common.Handler;
namespace Order.API.Features.Products.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<Result<IEnumerable<ProductResponseDto>>> GetAllAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient("Poduct");
            var response = await client.GetAsync($"{Common.Enum.HttpMethodType.ProductAPIBase}/api/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<Shared.HttpResponse>(apiContent);
            if (resp.IsSuccess)
            {
                var obj = JsonConvert.DeserializeObject<IEnumerable<ProductResponseDto>>(Convert.ToString(resp.Data));
                return await Result<IEnumerable<ProductResponseDto>>.SuccessAsync(obj, "Viewed Successfully", true);
            }

            return await Result<IEnumerable<ProductResponseDto>>.FaildAsync(false, "Not Viewed");
        }

    }
}
