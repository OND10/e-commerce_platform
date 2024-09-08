using eCommerceWebMVC.Services.TokenServices.Interface;
using eCommerceWebMVC.Shared;
using MicroserviceMVC.Common.Handler;
using MicroserviceMVC.Service.WebServices.Interface;
using Newtonsoft.Json;
using OnMapper;
using System.Net;
using System.Text;

namespace MicroserviceMVC.Service.WebServices.Implementation
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProviderService _tokenService;
        public BaseService(IHttpClientFactory httpClientFactory, ITokenProviderService tokenService)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
        }

        public async Task<Result<eCommerceWebMVC.Shared.HttpResponse>> SendAsync(eCommerceWebMVC.Shared.HttpRequest request, bool withBearer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MicroApi");
                HttpMethod httpMethod = request.apiType.ToHttpMethod();
                HttpRequestMessage message = new HttpRequestMessage(httpMethod, request.Url);
                message.Headers.Add("Accept", "application/json");

                if(withBearer)
                {
                    var token = _tokenService.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }

                if (request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = await client.SendAsync(message);

                if (!response.IsSuccessStatusCode)
                {
                    return await Result<eCommerceWebMVC.Shared.HttpResponse>.FaildAsync(false, response.ReasonPhrase);
                }

                var apiContent = await response.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<eCommerceWebMVC.Shared.HttpResponse>(apiContent);

                return await Result<eCommerceWebMVC.Shared.HttpResponse>.SuccessAsync(apiResponseDto, "Executed Successfully", true);
            }
            catch (Exception ex)
            {
                return await Result<eCommerceWebMVC.Shared.HttpResponse>.FaildAsync(false, $"{ex.Message}");
            }
        }
        //public async Task<Result<ResponseDTO>> SendGetAllAsync(RequestDTO request)
        //{
        //    try
        //    {
        //        HttpClient client = _httpClientFactory.CreateClient("MicroApi");
        //        HttpRequestMessage message = new();
        //        message.Headers.Add("Accept", "application/json");

        //        //Token
        //        //This is for Get data but if we want to post data so, we need to serialize data
        //        message.RequestUri = new Uri(request.Url);

        //        if (request.Data != null)
        //        {
        //            message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
        //        }

        //        HttpResponseMessage response = null;

        //        switch (request.apiType)
        //        {
        //            case SD.ApiType.Post:
        //                message.Method = HttpMethod.Post;
        //                break;

        //            case SD.ApiType.Delete:
        //                message.Method = HttpMethod.Delete;
        //                break;

        //            case SD.ApiType.Put:
        //                message.Method = HttpMethod.Put;
        //                break;

        //            default:
        //                message.Method = HttpMethod.Get;
        //                break;
        //        }
        //        response = await client.SendAsync(message);

        //        switch (response.StatusCode)
        //        {

        //            case HttpStatusCode.NotFound:
        //                return new Result<ResponseDTO> { IsSuccess = false, Message = " not Found" };
        //            case HttpStatusCode.Forbidden:
        //                return new Result<ResponseDTO> { IsSuccess = false, Message = " not Forbidden" };
        //            case HttpStatusCode.Unauthorized:
        //                return new Result<ResponseDTO> { IsSuccess = false, Message = " not Unauthorized" };
        //            case HttpStatusCode.InternalServerError:
        //                return new Result<ResponseDTO> { IsSuccess = false, Message = " 500" };

        //            default:
        //                var apicontent = await response.Content.ReadAsStringAsync();
        //                var apiresponseDto = JsonConvert.DeserializeObject<ResponseDTO>(apicontent);
        //                //if (apiresponseDto is not null)
        //                //{
        //                //    return new Response { DataPre = apiresponseDto, IsSuccess = true, Message = "Success" };
        //                //}
        //                //else
        //                //{
        //                //    return null;
        //                //}
        //                return await Result<ResponseDTO>.SuccessAsync(apiresponseDto, "Executed Successfully", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new ResponseDTO
        //        {
        //            Message = ex.Message.ToString(),
        //            IsSuccess = false
        //        };
        //        return await Result<ResponseDTO>.FaildAsync(false, $"{ex.Message}");
        //    }
        //}

        //public async Task<IResponse> SendGetAsync(RequestDTO request)
        //{
        //    try
        //    {
        //        HttpClient client = _httpClientFactory.CreateClient("MicroApi");
        //        HttpRequestMessage message = new();
        //        message.Headers.Add("Accept", "application/json");

        //        //Token
        //        //This is for Get data but if we want to post data so, we need to serialize data
        //        message.RequestUri = new Uri(request.Url);

        //        if (request.Data != null)
        //        {
        //            message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
        //        }

        //        HttpResponseMessage response = null;

        //        switch (request.apiType)
        //        {
        //            case SD.ApiType.Post:
        //                message.Method = HttpMethod.Post;
        //                break;

        //            case SD.ApiType.Delete:
        //                message.Method = HttpMethod.Delete;
        //                break;

        //            case SD.ApiType.Put:
        //                message.Method = HttpMethod.Put;
        //                break;

        //            default:
        //                message.Method = HttpMethod.Get;
        //                break;
        //        }
        //        response = await client.SendAsync(message);

        //        switch (response.StatusCode)
        //        {

        //            case HttpStatusCode.NotFound:
        //                return new Response<CouponRequestDTO> { IsSuccess = false, Message = " not Found" };
        //            case HttpStatusCode.Forbidden:
        //                return new Response<CouponRequestDTO> { IsSuccess = false, Message = " not Forbidden" };
        //            case HttpStatusCode.Unauthorized:
        //                return new Response<CouponRequestDTO> { IsSuccess = false, Message = " not Unauthorized" };
        //            case HttpStatusCode.InternalServerError:
        //                return new Response<CouponRequestDTO> { IsSuccess = false, Message = " 500" };

        //            default:
        //                var apicontent = await response.Content.ReadAsStringAsync();
        //                var apiresponseDto = JsonConvert.DeserializeObject<CouponRequestDTO>(apicontent);
        //                if (apiresponseDto is not null)
        //                {
        //                    return new Response<CouponRequestDTO> { Data=apiresponseDto, IsSuccess = true, Message = "Success" };
        //                }
        //                else
        //                {
        //                    return null;
        //                }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        var response = new Response<CouponRequestDTO>
        //        {
        //            Message = ex.Message.ToString(),
        //            IsSuccess = false
        //        };
        //        return response;
        //    }
        //}



        //private async Task<Result<ResponseDTO>> SendAsync(RequestDTO request)
        //{
        //    try
        //    {
        //        HttpClient client = _httpClientFactory.CreateClient("MicroApi");
        //        HttpMethod httpMethod = request.apiType.ToHttpMethod();
        //        HttpRequestMessage message = new HttpRequestMessage(httpMethod, request.Url);
        //        message.Headers.Add("Accept", "application/json");

        //        if (request.Data != null)
        //        {
        //            message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
        //        }

        //        HttpResponseMessage response = await client.SendAsync(message);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return await Result<ResponseDTO>.FaildAsync(false, response.ReasonPhrase);
        //        }

        //        var apiContent = await response.Content.ReadAsStringAsync();
        //        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);

        //        return await Result<ResponseDTO>.SuccessAsync(apiResponseDto, "Executed Successfully", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        return await Result<ResponseDTO>.FaildAsync(false, $"{ex.Message}");
        //    }
        //}


    }
}
