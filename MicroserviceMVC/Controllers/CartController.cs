using eCommerceWebMVC.Common.Enum;
using eCommerceWebMVC.Models.DTOs.CartDTOs.Request;
using eCommerceWebMVC.Models.DTOs.StripeDTOs.Request;
using eCommerceWebMVC.Services.CartServices.Interface;
using eCommerceWebMVC.Services.OrderServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace eCommerceWebMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            var response = await _cartService.GetAsync(userId);

            CartDto cart = new CartDto();
            if (response.IsSuccess && response.Data != null)
            {
                cart = response.Data.FirstOrDefault() ?? new CartDto();
            }

            return View(cart);
        }

        public async Task<CartDto> LoadCart()
        {
            
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            var response = await _cartService.GetAsync(userId);
            response.Data.First().CartHeaderResponse.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
            if (response.IsSuccess)
            {
                return response.Data.First();
            }
            return response.Data.First();

        }

        public async Task<IActionResult> Delete(int cartDetailsId)
        {

            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            var response = await _cartService.DeleteAsync(cartDetailsId);
            if (response.IsSuccess)
            {
                TempData["msg"] = "Cart is updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cart)
        {

            var response = await _cartService.ApplyCouponAsync(cart);
            if (response.IsSuccess)
            {
                TempData["msg"] = "Coupon is applied Successfully";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cart)
        {
            cart.CartHeaderResponse.CouponCode = "";
            var response = await _cartService.RemoveCouponAsync(cart);
            if (response.IsSuccess)
            {
                TempData["msg"] = "Coupon is demolished Successfully";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cart)
        {
            CartDto cartobj = await LoadCart(); 
            var response = await _cartService.EmailCart(cartobj);
            if (response.IsSuccess)
            {
                TempData["msg"] = "Email will be processed and sent Successfully using the busService";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult>Checkout()
        {
            return View(await LoadCart());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {

            var cart = await LoadCart();
            cart.CartHeaderResponse.Name = cartDto.CartHeaderResponse.Name;
            cart.CartHeaderResponse.PhoneNumber = cartDto.CartHeaderResponse.PhoneNumber;
            cart.CartHeaderResponse.Email = cartDto.CartHeaderResponse.Email;

            var response = await _orderService.CreatAsync(cart);
            if (response != null && response.IsSuccess)
            {
                //Prepare the httpRequest
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                //Implement the payment using Stripe
                StripeRequestDto request = new()
                {
                    ApprovedUrl = domain + "cart/Confirmation?orderId=" + response.Data.Id,
                    CancelUrl = domain + "cart/Checkout",
                    OrderHeader = response.Data
                };

                var stripeResponse = await _orderService.CreateStripeSession(request);

                if (stripeResponse.IsSuccess)
                {
                    Response.Headers.Add("Location", stripeResponse.Data.StripeSessionUrl);
                    return new StatusCodeResult(303);
                }
            }

            return View();
        }

        public async Task<IActionResult>Confirmation(int orderId)
        {
            var response = await _orderService.VerifyStripeSession(orderId);
            if(response != null && response.IsSuccess)
            {
                if(response.Data.Status == StatusEnum.Status_Approved)
                {
                    return View(orderId);
                }
            }
            return View(orderId);
        }

    }
}
