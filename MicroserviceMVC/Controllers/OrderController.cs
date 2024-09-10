using eCommerceWebMVC.Common.Enum;
using eCommerceWebMVC.Models.DTOs.OrderDTOs;
using eCommerceWebMVC.Services.OrderServices.Interface;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace eCommerceWebMVC.Controllers
{
    [Route("Order")]
    public class OrderController : Controller
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            //var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            //var list = await _service.GetAllOrders(userId);
            return View();
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            string usrId = "";
            string adminRole = UserRolesEnum.Admin.ToString();

            // Check if the user is in the Admin role
            if (User.IsInRole(adminRole))
            {
                usrId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            }

            // Fetch orders based on the user ID (admin or specific user)
            var response = _service.GetAllOrders(usrId).GetAwaiter().GetResult();

            // Check if the response is successful
            if (response.IsSuccess)
            {
                var jsonData = new
                {
                    data = response.Data,
                };

                return Json(jsonData); // Return data for DataTable
            }
            else
            {
                // Return an empty list if the operation failed
                return Json(new { data = new List<OrderHeaderResponseDto>() });
            }
        }

        [HttpGet("Details")]
        public async Task<IActionResult>Details(int id)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;

            var response = await _service.GetOrderById(id);

            if (!User.IsInRole(UserRolesEnum.Admin.ToString()) && userId != response.Data.UserId)
            {
                return NotFound();
            }
               
            return View(response.Data);
        }
    }
}
