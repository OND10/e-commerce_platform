using Email.API.Features.DTOs.CartDTOs;
using Email.API.Message;

namespace Email.API.Services
{
    public interface IEmailService
    {
        Task EmailLoggingCart(CartDto cart);
        Task CreateUserAccountLog(string email);
        Task LogOrderPlaced(RewardsMessage rewardsMessage);
    }
}
