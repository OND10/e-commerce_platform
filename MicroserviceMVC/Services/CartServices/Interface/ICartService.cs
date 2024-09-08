using Microsoft.AspNetCore.Mvc;
using MicroserviceMVC.Common.Handler;
using eCommerceWebMVC.Models.DTOs.CartDTOs.Request;

namespace eCommerceWebMVC.Services.CartServices.Interface
{
    public interface ICartService
    {
        public Task<Result<IEnumerable<CartDto>>> GetAsync(string userId);
        public Task<Result<bool>> ApplyCouponAsync(CartDto cart);
        public Task<Result<bool>> RemoveCouponAsync(CartDto cart);
        public Task<Result<CartDto>> CreateAsync(CartDto cartDto);
        public Task<Result<bool>> DeleteAsync(int cartDetailsId);
        public Task<Result<bool>> EmailCart(CartDto cart);
    }
}
