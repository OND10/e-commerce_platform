using eCommerceWebMVC.Models;
using eCommerceWebMVC.Models.DTOs.AuthDTOs.Request;
using eCommerceWebMVC.Models.DTOs.AuthDTOs.Response;
using MicroserviceMVC.Common.Handler;

namespace eCommerceWebMVC.Services.AuthServices.Interface
{
    public interface IAuthService
    {
        Task<Result<LoginResponseDTO>> Login(LoginRequestDTO request);
        Task<Result<UserDTO>> Register(RegisterRequestDTO request);
        Task<Result<bool>> AddUserToRole(UserRoleRequestDTO request);
        Task<Result<IList<string>>> GetUserRolesAsync(string userId);
    }

}
