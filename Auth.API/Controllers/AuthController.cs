using Auth.API.Common.Handler;
using Auth.API.Models.DTOs;
using Auth.API.Models.DTOs.Request;
using Auth.API.Models.DTOs.Response;
using Auth.API.Services.Interface;
using MessageBus.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMessageBusService _messageBus;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IMessageBusService messageBus, IConfiguration configuration)
        {
            _authService = authService;
            _messageBus = messageBus;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<Result<UserDTO>> Register([FromBody] RegisterRequestDTO request)
        {
            var result = await _authService.Register(request);
            if (result.IsSuccess)
            {
                await _messageBus.PublishMessage(request.Email, _configuration.GetValue<string>("TopicAndQueueNames:UserLoggingQueue"));
                return await Result<UserDTO>.SuccessAsync(result.Data, "User Added Successfully", true);
            }

            return await Result<UserDTO>.FaildAsync(false, "User not Added");
        }

        [HttpPost]
        [Route("login")]
        public async Task<Result<LoginResponseDTO>> Login([FromBody] LoginRequestDTO request)
        {
            var result = await _authService.Login(request);
            if (result.IsSuccess)
            {
                return await Result<LoginResponseDTO>.SuccessAsync(result.Data, "Logged Successfully", true);
            }

            return await Result<LoginResponseDTO>.FaildAsync(false, "Username or Password are in correct");
        }

        [HttpPost]
        [Route("roleAssign")]
        public async Task<Result<bool>> UserRole([FromBody] UserRoleRequestDTO request)
        {
            var result = await _authService.AddUserToRole(request);
            return await Result<bool>.SuccessAsync(result.Data, "Role is Added Successfully", true);
        }

        [HttpGet("GetUserRoles/{userId}")]
        public async Task<Result<IList<string>>> Get(string userId) 
        { 
            var response = await _authService.GetUserRolesAsync(userId);

            if(response.IsSuccess)
            {
                return await Result<IList<string>>.SuccessAsync(response.Data, "Get all Roles Successfully", true);
            }

            return await Result<IList<string>>.FaildAsync(false, "Operation faild in fetching Roles");
        }

    }
}
