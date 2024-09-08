using eCommerceWebMVC.Models.DTOs.CouponDTOs.Request;
using MicroserviceMVC.Service.CouponServices.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MicroserviceMVC.Controllers
{
    public class CouponController : Controller
    {
        private ICouponService _service;
        public CouponController(ICouponService service)
        {
            _service = service;
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

        public async Task<IActionResult> Find(int id)
        {
            var response = await _service.GetByIdAsync(id);
            if (response.IsSuccess)
            {
                return Json(response.Data);
            }
            return Json(response.Message);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return await Task.FromResult<IActionResult>(View());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CouponRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _service.CreaAsync(model);
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(response.Message);
            }
            else
            {
                return View(model);
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

    }
}
