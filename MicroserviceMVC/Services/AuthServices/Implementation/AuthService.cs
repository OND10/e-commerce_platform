using eCommerceWebMVC.Models;
using eCommerceWebMVC.Models.DTOs.AuthDTOs.Request;
using eCommerceWebMVC.Models.DTOs.AuthDTOs.Response;
using eCommerceWebMVC.Services.AuthServices.Interface;
using MicroserviceMVC.Common.Enum;
using MicroserviceMVC.Common.Handler;
using MicroserviceMVC.Service.WebServices.Interface;
using Newtonsoft.Json;
using static MicroserviceMVC.Common.Enum.HttpMethodType;
using System.Reflection;

namespace eCommerceWebMVC.Services.AuthServices.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<Result<bool>> AddUserToRole(UserRoleRequestDTO request)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = ApiType.Post,
                Url = $"{HttpMethodType.AuthAPIBase}/api/auth/roleAssign",
                Data = request
            });

            if (result.IsSuccess)
            {
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

        public async Task<Result<IList<string>>> GetUserRolesAsync(string userId)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = HttpMethodType.ApiType.Get,
                Url = $"{HttpMethodType.AuthAPIBase}/api/auth/GetUserRoles/{userId}",
            });

            if (result.IsSuccess)
            {
                if (result.Response.Data is not null)
                {
                    var data = JsonConvert.DeserializeObject<IList<string>>(result.Response.Data.ToString());
                    return await Result<IList<string>>.SuccessAsync(data, result.Message, true);
                }
                else
                {
                    return await Result<IList<string>>.FaildAsync(false, "result.Response.Data is null");
                }
            }
            else
            {
                return await Result<IList<string>>.FaildAsync(false, result.Message);
            }
        }

        public async Task<Result<LoginResponseDTO>> Login(LoginRequestDTO request)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = ApiType.Post,
                Url = $"{HttpMethodType.AuthAPIBase}/api/auth/login",
                Data = request
            },withBearer:false);

            if (result.IsSuccess && result.Response.Data is not null)
            {
                var data = JsonConvert.DeserializeObject<LoginResponseDTO>(result.Response.Data.ToString());
                return await Result<LoginResponseDTO>.SuccessAsync(data, "User logged Successfully", true);
            }
            else
            {
                return await Result<LoginResponseDTO>.FaildAsync(false, "Username or Password are incorrect");
            }
        }

        public async Task<Result<UserDTO>> Register(RegisterRequestDTO request)
        {
            var result = await _baseService.SendAsync(new eCommerceWebMVC.Shared.HttpRequest
            {
                apiType = ApiType.Post,
                Url = $"{HttpMethodType.AuthAPIBase}/api/auth/register",
                Data = request
            }, withBearer: false);

            if (result.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<UserDTO>(result.Response.Data.ToString());
                return await Result<UserDTO>.SuccessAsync(data, "User is Created Successfully", true);
            }
            else
            {
                return await Result<UserDTO>.FaildAsync(false, result.Message);
            }
        }
    }
}
