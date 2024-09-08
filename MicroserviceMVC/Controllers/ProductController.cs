using eCommerceWebMVC.Common.Helpers;
using eCommerceWebMVC.Models.DTOs.CartDTOs.Request;
using eCommerceWebMVC.Models.DTOs.CartDTOs.Response;
using eCommerceWebMVC.Models.DTOs.ProductDTOs.Request;
using eCommerceWebMVC.Models.DTOs.ProductDTOs.Response;
using eCommerceWebMVC.Services.CartServices.Interface;
using eCommerceWebMVC.Services.ProductServices.Interface;
using eCommerceWebMVC.Services.SignalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using OnMapper;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ZXing.QrCode.Internal;

namespace eCommerceWebMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        private readonly IImageUploadService<ProductRequestDto> _imageUploadService;
        private readonly ICartService _cartService;
        private readonly IHubContext<ProductHub> _hubContext;
        public ProductController(IProductService service, IImageUploadService<ProductRequestDto> imageUploadService, ICartService cartService,
            IHubContext<ProductHub> hubContext)
        {
            _service = service;
            _imageUploadService = imageUploadService;
            _cartService = cartService;
            _hubContext = hubContext; 
        }
        public async Task<IActionResult> Index()
        {
            var response = await _service.GetAllAsync();
            if (response.IsSuccess)
            {
                return View(response.Data);
            }

            TempData["error"] = "No Coupons are found";
            return Json(response.Message);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var cateList = new List<SelectListItem>()
            {
                new SelectListItem{Text = "Phones", Value = "Phones"},
                new SelectListItem{Text = "Laptops", Value = "Laptops"},
                new SelectListItem{Text = "Clothes", Value = "Clothes"},
                new SelectListItem{Text = "Food", Value = "Food"},
            };

            ViewBag.catelist = cateList;
            return await Task.FromResult<IActionResult>(View());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductRequestDto request)
        {
            request.ImageUrl = "Once Again";
            try
            {
                string uniqueFileName = UploadFile(request);
                request.ImageUrl = uniqueFileName;

                var response = await _service.CreaAsync(request);
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(response.Message);
            }
            catch
            {

                var cateList = new List<SelectListItem>()
                {
                    new SelectListItem{Text = "Phones", Value = "Phones"},
                    new SelectListItem{Text = "Laptops", Value = "Laptops"},
                    new SelectListItem{Text = "Clothes", Value = "Clothes"},
                    new SelectListItem{Text = "Food", Value = "Food"},
                };

                ViewBag.catelist = cateList;
                return View(request);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                TempData["msg"] = "You should enter a correct Id";
                return RedirectToAction(nameof(Index));
            }
            var edit = await _service.GetByIdAsync(id);
            if (edit == null)
            {

                TempData["msg"] = "Id is null ";
                return RedirectToAction("Index");
            }
            else
            {
                var cateList = new List<SelectListItem>()
            {
                new SelectListItem{Text = "Phones", Value = "Phones"},
                new SelectListItem{Text = "Laptops", Value = "Laptops"},
                new SelectListItem{Text = "Clothes", Value = "Clothes"},
                new SelectListItem{Text = "Food", Value = "Food"},
            };

                ViewBag.catelist = cateList;
                return await Task.FromResult<IActionResult>(View(edit.Data));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductRequestDto request)
        {
            try
            {

                var mapper = new OnMapping();
                var modelRequest = await mapper.Map<UpdateProductRequestDto, ProductRequestDto>(request);
                string uniqueFileName = UploadFile(modelRequest.Data);
                modelRequest.Data.ImageUrl = uniqueFileName;
                var response = await _service.UpdateAsync(request.Id, modelRequest.Data);
                if (response.IsSuccess)
                {

                    TempData["msg"] = "Your Product equips has been Updated Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    var cateList = new List<SelectListItem>()
                    {
                         new SelectListItem{Text = "Phones", Value = "Phones"},
                         new SelectListItem{Text = "Laptops", Value = "Laptops"},
                         new SelectListItem{Text = "Clothes", Value = "Clothes"},
                         new SelectListItem{Text = "Food", Value = "Food"},
                    };

                    ViewBag.catelist = cateList;
                    return View(request);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Exc happend in Edit Function");
                Console.WriteLine($"{ex.Message}");
                TempData["msg"] = "System is under Service";
                var cateList = new List<SelectListItem>()
                {
                     new SelectListItem{Text = "Phones", Value = "Phones"},
                     new SelectListItem{Text = "Laptops", Value = "Laptops"},
                     new SelectListItem{Text = "Clothes", Value = "Clothes"},
                     new SelectListItem{Text = "Food", Value = "Food"},
                };

                ViewBag.catelist = cateList;
                return View(request);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(id);
            if (response.IsSuccess)
            {
                TempData["msg"] = "Coupun is deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _service.GetByIdAsync(id);
            if (response.IsSuccess)
            {
                return View(response.Data);
            }
            return Json(response.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Details(ProductResponseDto request)
        {
            CartDto cartDto = new CartDto
            {
                CartHeaderResponse = new CartHeaderResponseDto
                {
                    UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value
                }
            };
            CartDetailsResponseDto cartDetailsResponse = new CartDetailsResponseDto()
            {
                Count = request.NumberofProduct,
                ProductId = request.Id, 
                
            };

            List<CartDetailsResponseDto> cartDetails = new()
            {
                cartDetailsResponse
            };

            cartDto.CartDetailsResponse = cartDetails;
            var response = await _cartService.CreateAsync(cartDto);
            if (response.IsSuccess)
            {
                TempData["msg"] = "Item has been added to the shopping cart";
                return RedirectToAction("Index","Home");
            }
            return View(request);
        }

        private string UploadFile(ProductRequestDto request)
        {
            return _imageUploadService.ImageUpload(request, request.FrontIamge);
        }

        [HttpGet]
        public async Task<IActionResult> Generatecode(int id)
        {
            var generate = await _service.GetByIdAsync(id);
            return View(generate.Data);
        }

        [HttpPost]
        public async Task<ViewResult> Generatecode(ProductRequestDto request)
        {

            ViewData["name"] = request.Name;
            byte[] qrCodeImage = await GenerateQrCode(request);
            ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(qrCodeImage);
            TempData["msg"] = "QR code for this product has been successfully Generated";
            return View("Generatecode");
        }

        private Task<byte[]> GenerateQrCode(ProductRequestDto request)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qr = new QRCodeGenerator();

                // Combine the product name and price into a single string
                string productInfo = $"Name: {request.Name}, Price: {request.Price}";

                // Convert the string to a byte array
                byte[] byteProductInfo = Encoding.UTF8.GetBytes(productInfo);

                // Generate the QR code from the byte array
                QRCodeData qRCodeData = qr.CreateQrCode(productInfo, QRCodeGenerator.ECCLevel.Q);
                Common.Helpers.QRCode qRCode = new Common.Helpers.QRCode(qRCodeData);

                using (Bitmap bitmap = qRCode.GetGraphic(20))
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return Task.FromResult(ms.ToArray());
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePrice(int productId, int numberOfProduct)
        {
            // Assuming newPrice is calculated based on numberOfProduct
            int newPrice = CalculateNewPrice(numberOfProduct);

            // Send real-time update to clients
            await _hubContext.Clients.All.SendAsync("ReceivePriceUpdate", productId, newPrice);

            return Ok(newPrice);
        }


        private int CalculateNewPrice(int numberofProduct)
        {
            return numberofProduct * 10;
        }

    }
}
