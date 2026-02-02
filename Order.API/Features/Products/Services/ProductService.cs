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
                Error ProductsFetchError = new(response.StatusCode.ToString(), responseDto?.Error.Description, SharedKernels.ErrorType.Failure);
                return Result.Failure<IEnumerable<ProductResponseDto>>(ProductsFetchError);
            }
            // If HTTP status is not success
            var errorContent = await response.Content.ReadAsStringAsync();
            Error ApiFailedError = new(response.StatusCode.ToString(), errorContent, SharedKernels.ErrorType.Failure);
            return Result.Failure<IEnumerable<ProductResponseDto>>(ApiFailedError);
        }

    }
}
