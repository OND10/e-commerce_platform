using Auth.API.Common.Handler;
using Auth.API.Models.DTOs;
using Auth.API.Models.DTOs.Request;
using Auth.API.Models.DTOs.Response;

namespace Auth.API.Services.Interface
{
    public interface IAuthService
    {
        Task<Result<LoginResponseDTO>> Login(LoginRequestDTO request);
        Task<Result<UserDTO>> Register(RegisterRequestDTO request);
        Task<Result<bool>> AddUserToRole(UserRoleRequestDTO request);
        Task<Result<IList<string>>> GetUserRolesAsync(string userId);


    }
}
