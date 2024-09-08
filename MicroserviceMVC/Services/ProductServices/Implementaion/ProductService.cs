using eCommerceWebMVC.Models.DTOs.ProductDTOs.Request;
using eCommerceWebMVC.Models.DTOs.ProductDTOs.Response;
using eCommerceWebMVC.Services.ProductServices.Interface;
using MicroserviceMVC.Common.Enum;
using MicroserviceMVC.Common.Handler;
using MicroserviceMVC.Service.WebServices.Interface;
using Newtonsoft.Json;
using static MicroserviceMVC.Common.Enum.HttpMethodType;

namespace eCommerceWebMVC.Services.ProductServices.Implementaion
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<Result<ProductResponseDto>> CreaAsync(ProductRequestDto model)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = ApiType.Post,
                Url = $"{HttpMethodType.ProductAPIBase}/api/product/",
                Data = model
            });

            if (result.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<ProductResponseDto>(result.Response.Data.ToString());
                return await Result<ProductResponseDto>.SuccessAsync(data, "Created Successfully", true);
            }
            else
            {
                return await Result<ProductResponseDto>.FaildAsync(false, result.Message);
            }

        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = HttpMethodType.ApiType.Delete,
                Url = $"{HttpMethodType.ProductAPIBase}/api/product/{id}"
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

        public async Task<Result<IEnumerable<ProductResponseDto>>> GetAllAsync()
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = HttpMethodType.ApiType.Get,
                Url = $"{HttpMethodType.ProductAPIBase}/api/product/"
            }, withBearer: true);

            if (result.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<IEnumerable<ProductResponseDto>>(result.Response.Data.ToString());
                return await Result<IEnumerable<ProductResponseDto>>.SuccessAsync(data, "Viewed Successfully", true);
            }
            else
            {
                return await Result<IEnumerable<ProductResponseDto>>.FaildAsync(false, result.Message);
            }
        }

        public async Task<Result<ProductResponseDto>> GetByIdAsync(int id)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = HttpMethodType.ApiType.Get,
                Url = $"{HttpMethodType.ProductAPIBase}/api/product/{id}"
            });
            if (result.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<ProductResponseDto>(result.Response.Data.ToString());
                return await Result<ProductResponseDto>.SuccessAsync(data, "Found Successfully", true);
            }

            return await Result<ProductResponseDto>.FaildAsync(false, result.Message);
        }

        public async Task<Result<ProductResponseDto>> UpdateAsync(int id, ProductRequestDto model)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = ApiType.Put,
                Url = $"{HttpMethodType.ProductAPIBase}/api/product/{id}",
                Data = model
            });

            if (result.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<ProductResponseDto>(result.Response.Data.ToString());
                return await Result<ProductResponseDto>.SuccessAsync(data, "Updated Successfully", true);
            }
            else
            {
                return await Result<ProductResponseDto>.FaildAsync(false, result.Message);
            }
        }
    }
}
