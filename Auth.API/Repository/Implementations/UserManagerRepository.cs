using Auth.API.Contract.Interfaces;
using Auth.API.Data;
using Auth.API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Repository.Implementations
{
    public class UserManagerRepository : IUserManagerRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserManagerRepository(UserManager<ApplicationUser> userManager, AppDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            return result;
        }

        public async Task<bool> AssignRoleToUser(string email, string roleName)
        {
            var result = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (result != null)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await AddUserToRoleAsync(result, roleName);
                return true;
            }
            return false;
        }

        public async Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password)
        {

            var result = await _userManager.CheckPasswordAsync(user, password);
            return result;
        }

        public async Task<IdentityResult> ConfirmUserEmailAsync(ApplicationUser user, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result;
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<ApplicationUser> FindUserByEmailAsync(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return result;
        }

        public async Task<ApplicationUser> FindUserByIdAsync(string userId)
        {
            var result = await _userManager.FindByIdAsync(userId);
            return result;
        }

        public async Task<ApplicationUser> FindUserByNameAsync(string userName)
        {
            var result = await _userManager.FindByNameAsync(userName);
            return result;
        }

        public async Task<string> GenerateUserEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var result = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return result;
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            var result = await _userManager.GetRolesAsync(user);
            return result;
        }

        public async Task<IList<string>>GetUserRolesAsync(string userId)
        {

            //Old code take to much to query form DB even slows the performance
            //var userRolesList = new List<string>();
            //var userRole = await _context.UserRoles.Where(u => u.UserId == userId).ToListAsync();

            //foreach (var role in userRole)
            //{
            //    var roles = await _context.Roles.Where(r => r.Id == role.RoleId).ToListAsync();
            //    foreach(var item in roles)
            //    {
            //        userRolesList.Add(item.Name);
            //    }
            //}

            // a good way to return userRoles with high performance and scalability.
            var userRoles = await (from ur in _context.UserRoles
                                   join r in _context.Roles on ur.RoleId equals r.Id
                                   where ur.UserId == userId
                                   select r.Name)
                                  .ToListAsync();

            return userRoles;
        }

    }
}
