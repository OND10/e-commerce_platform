using eCommerceWebMVC.Common.Enum;
using eCommerceWebMVC.Models.DTOs.AuthDTOs.Request;
using eCommerceWebMVC.Models.DTOs.AuthDTOs.Response;
using eCommerceWebMVC.Services.AuthServices.Interface;
using eCommerceWebMVC.Services.TokenServices.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace eCommerceWebMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProviderService _tokenService;
        public AuthController(IAuthService authService, ITokenProviderService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return await Task.FromResult<IActionResult>(View());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            var response = await _authService.Login(request);
            
            if (response.IsSuccess)
            {
                await SignInUser(response.Data);
                _tokenService.SetToken(response.Data.Token);
                return RedirectToAction("Index", "Home");
            }

            TempData["error"] = response.Message; 
            //ModelState.AddModelError("CustomerError", response.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = UserRolesEnum.Admin.ToString(), Value = UserRolesEnum.Admin.ToString()},
                new SelectListItem{Text = UserRolesEnum.Customer.ToString(), Value = UserRolesEnum.Customer.ToString()},
            };

            ViewBag.rolelist = roleList;
            return await Task.FromResult<IActionResult>(View());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDTO request)
        {
            var response = await _authService.Register(request);
            if (response.IsSuccess)
            {
                if (string.IsNullOrEmpty(request.Role))
                {
                    request.Role = UserRolesEnum.Customer.ToString();
                }
                var userRolemodel = new UserRoleRequestDTO
                {
                    Email = request.Email,
                    roleName = request.Role,
                };
                var roleResponse = await _authService.AddUserToRole(userRolemodel);
                if (roleResponse.IsSuccess)
                {
                    TempData["msg"] = "Account is created Successfully";
                    return RedirectToAction("Login", "Auth");
                }
            }

            TempData["error"] = "Can't add User";
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = UserRolesEnum.Admin.ToString(), Value = UserRolesEnum.Admin.ToString()},
                new SelectListItem{Text = UserRolesEnum.Customer.ToString(), Value = UserRolesEnum.Customer.ToString()},
            };

            ViewBag.rolelist = roleList;

            return Json(request);
        }

        //[HttpGet]
        //public async Task<IActionResult> AssignUserRole()
        //{
        //    return await Task.FromResult<IActionResult>(View());
        //}

        //[HttpPost]
        //public async Task<IActionResult> AssignUserRole(UserRoleRequestDTO request)
        //{
        //    var response = await _authService.AddUserToRole(request);
        //    if (response.IsSuccess)
        //    {
        //        return View(response.Data);
        //    }

        //    TempData["error"] = "Can't add Role to User";
        //    return Json(response.Message);
        //}

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenService.ClearToken();
            return RedirectToAction("Login");
        }

        private async Task SignInUser(LoginResponseDTO response)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(response.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim
                (new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim
                (new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim
                (new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim
                (new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim
                (new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principle = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
        }
    }
}
