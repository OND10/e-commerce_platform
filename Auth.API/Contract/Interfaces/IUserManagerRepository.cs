using Auth.API.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.Contract.Interfaces
{
    public interface IUserManagerRepository
    {
        public Task<ApplicationUser> FindUserByEmailAsync(string email);
        public Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password);
        public Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        public Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        public Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string role);
        public Task<string> GenerateUserEmailConfirmationTokenAsync(ApplicationUser user);
        public Task<IdentityResult> ConfirmUserEmailAsync(ApplicationUser user, string token);
        public Task<ApplicationUser> FindUserByIdAsync(string userId);
        public Task<ApplicationUser> FindUserByNameAsync(string userName);
        public Task<bool> AssignRoleToUser(string email, string roleName);
        Task<IList<string>> GetUserRolesAsync(string userId);
    }
}
