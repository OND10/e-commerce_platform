using SharedKernels.Results;
using Newtonsoft.Json;
using Order.API.Features.Products.Dtos.Response;
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
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var responseDto = JsonConvert.DeserializeObject<Result<IEnumerable<ProductResponseDto>>>(content);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    return responseDto;
                }
                // If HTTP status is success but the internal Result indicates failure
                return Result.Failure<IEnumerable<ProductResponseDto>>(responseDto?.Error.Description ?? "Failed to retrieve products from API.");
            }
            // If HTTP status is not success
            var errorContent = await response.Content.ReadAsStringAsync();
            return Result.Failure<IEnumerable<ProductResponseDto>>($"API call failed with status code {response.StatusCode}. Details: {errorContent}");
        }

    }
}
