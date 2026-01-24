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
        public IActionResult GetAll(string status)
        {
            string usrId = "";
            string adminRole = UserRolesEnum.Admin.ToString();

            // Check if the user is in the Admin role
            if (!User.IsInRole(adminRole))
            {
                usrId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            }


            // Fetch orders based on the user ID (admin or specific user)
            var response = _service.GetAllOrders(usrId).GetAwaiter().GetResult();

            // Check if the response is successful
            if (response.IsSuccess)
            {
                switch (status)
                {
                    case "approved":
                        response.Data.Where(u => u.Status == StatusEnum.Status_Approved).ToList();
                        break;
                    case "readyforpickup":
                        response.Data.Where(u => u.Status == StatusEnum.Status_ReadyForPickup).ToList();
                        break;
                    case "cancelled":
                        response.Data.Where(u => u.Status == StatusEnum.Status_Cancelled).ToList();
                        break;
                    default:
                        break;
                }
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

        [HttpPost("OrderReadyForPickUp")]
        public async Task<IActionResult> OrderReadyForPickUp(int orderId)
        {
            var response = await _service.UpdateOrderStatus(orderId, StatusEnum.Status_ReadyForPickup);

            if (response.IsSuccess)
            {
                TempData["success"] = "Status Updated Successfully";
                return RedirectToAction(nameof(Details), new { id = orderId });
            }

            return View();
        }


        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            var response = await _service.UpdateOrderStatus(orderId, StatusEnum.Status_Completed);

            if (response.IsSuccess)
            {
                TempData["success"] = "Status Updated Successfully";
                return RedirectToAction(nameof(Details), new { id = orderId });
            }

            return View();
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var response = await _service.UpdateOrderStatus(orderId, StatusEnum.Status_Cancelled);

            if (response.IsSuccess)
            {
                TempData["success"] = "Status Updated Successfully";
                return RedirectToAction(nameof(Details), new { id = orderId });
            }

            return View();
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;

            var response = await _service.GetOrderById(id);


            if (!User.IsInRole(UserRolesEnum.Admin.ToString()) && userId != response.Data.UserId)
            {
                return View(response.Data);
            }
            return View(response.Data);
        }
    }
}
