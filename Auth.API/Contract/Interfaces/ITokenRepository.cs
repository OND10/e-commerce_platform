using Auth.API.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.Contract.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJWTTokenAsync(ApplicationUser user, IEnumerable<string> roles);
    }
}
