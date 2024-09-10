using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using eCommerceWebMVC.Services;
using eCommerceWebMVC.Services.AuthServices.Interface;

// Class should end with ViewComponent
public class UserRolesViewComponent : ViewComponent
{
    private readonly IAuthService _authService;

    public UserRolesViewComponent(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string userId)
    {
        var response = await _authService.GetUserRolesAsync(userId);

        if (response.IsSuccess && response.Data != null)
        {
            return View(response.Data); // Assuming Data is a list of roles
        }

        return View(new List<string>()); // Return an empty list if no roles are found
    }
}
