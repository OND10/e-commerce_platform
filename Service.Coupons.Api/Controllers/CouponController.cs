using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnMapper;
using OnMapper.Common.Exceptions;
using Service.Coupons.Api.Model;
using Service.Coupons.Api.Model.DTOs;
using Service.Coupons.Api.Repositories;
using Service.Coupons.Api.Services.Interface;
using Stripe;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Service.Coupons.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _service;
        public CouponController(ICouponService service)
        {
            _service = service;
        }

        // GET: api/<CouponController>
        [HttpGet]
        public async Task<Result<IEnumerable<CouponResponseDTO>>> Get()
        {
            var result = await _service.GetAllAsync();
            return await Result<IEnumerable<CouponResponseDTO>>.SuccessAsync(result.Data, "Viewed Successfully", true);
        }

        //GET api/<CouponController>/5
        //[HttpGet("{id}")]
        //[Authorize(Roles = "ADMIN")]
        //public async Task<Result<CouponResponseDTO>> GetId(int id)
        //{
        //    var findId = await _service.GetByIdAsync(id);
        //    return await Result<CouponResponseDTO>.SuccessAsync(findId.Data, "Found Successfully", true);
        //}


        [HttpGet("{code}")]
        public async Task<Result<CouponResponseDTO>> GetCode(string code)
        {
            var findCode = await _service.GetByCodeAsync(code);
            return await Result<CouponResponseDTO>.SuccessAsync(findCode.Data, "Found Successfullt", true);
        }

        //POST api/<CouponController>
        [HttpPost]
        public async Task<Result<CouponResponseDTO>> Post([FromBody] CouponRequestDTO model)
        {
            try
            {
                var add = await _service.CreaAsync(model);

                var options = new Stripe.CouponCreateOptions
                {
                    AmountOff = (long)(model.DiscountAmount * 100),
                    Name = model.CouponCode,
                    Currency = "usd",
                    // Id is optional; let Stripe generate it
                };

                var service = new Stripe.CouponService();
                var stripeCoupon = await service.CreateAsync(options);

                // Store the Stripe coupon ID in your database
                add.Data.StripeCouponId = stripeCoupon.Id;

                var request = new UpdateCouponRequestDTO()
                {
                    CouponId = add.Data.CouponId,
                    CouponCode = add.Data.CouponCode,
                    DiscountAmount = add.Data.DiscountAmount,
                    MinAmount = add.Data.MinAmount,
                    StripeCouponId = add.Data.StripeCouponId,
                };

                await _service.UpdateAsync(request);

                // Log created coupon details
                Console.WriteLine($"Created Stripe Coupon: {stripeCoupon.Id}");

                return await Result<CouponResponseDTO>.SuccessAsync(add.Data, "Created Successfully", true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating coupon: {ex.Message}");
                throw new ArgumentNullException(nameof(model), ex);
            }


        }

        //PUT api/<CouponController>/5

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<Result<CouponResponseDTO>> Put([FromRoute] int id, [FromBody] CouponRequestDTO model)
        {
            try
            {
                var mapper = new OnMapping();
                var mappedRequestModel = await mapper.Map<CouponRequestDTO, UpdateCouponRequestDTO>(model);
                mappedRequestModel.Data.CouponId = id;
                var updateResult = await _service.UpdateAsync(mappedRequestModel.Data);
                return await Result<CouponResponseDTO>.SuccessAsync(updateResult.Data, "Updated Successfully", true);
            }
            catch (Exception)
            {
                return await Result<CouponResponseDTO>.FaildAsync(true, "Not Updated");
            }
        }

        // DELETE api/<CouponController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var delete = await _service.GetByIdAsync(id);
                if (delete == null)
                {
                    return await Result<bool>.FaildAsync(false, "Not Deleted");
                }

                var stripeCouponId = delete.Data.StripeCouponId;  // Use the Stripe Coupon ID
                                                                  // Log the coupon code to be deleted
                Console.WriteLine($"Deleting coupon: {stripeCouponId}");

                var result = await _service.DeleteAsync(id);

                var service = new Stripe.CouponService();
                var stripeDeleteResponse = await service.DeleteAsync(stripeCouponId);
                // Log delete response
                Console.WriteLine($"Deleted Stripe Coupon: {stripeDeleteResponse.Id}");

                return await Result<bool>.SuccessAsync(result.Data, "Deleted Successfully", true);
            }
            catch (StripeException stripeEx)
            {
                // Log Stripe-specific exceptions
                Console.WriteLine($"Stripe error: {stripeEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Log other exceptions
                Console.WriteLine($"General error: {ex.Message}");
                throw new ArgumentNullException(nameof(id), ex);
            }

        }
    }
}
