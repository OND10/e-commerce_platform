using MediatR;
using Microsoft.EntityFrameworkCore;
using OnMapper;
using ShoppingCart.API.Common.Handler;
using ShoppingCart.API.DataBase;
using ShoppingCart.API.Entities;
using ShoppingCart.API.Features.Coupons;
using ShoppingCart.API.Features.DTOs.CartDetailsDTOs.Response;
using ShoppingCart.API.Features.DTOs.CartDTOs;
using ShoppingCart.API.Features.DTOs.CartHeaderDTOs.Response;
using ShoppingCart.API.Features.Products;

namespace ShoppingCart.API.Features.Carts.Requests.Queries.GetCarts
{
    public class GetCartsQueryHandler : IRequestHandler<GetCartsQuery, Result<IEnumerable<CartDto>>>
    {
        private readonly AppDbContext _context;
        private readonly OnMapping _mapper;
        private readonly IProductService _productService;
        private readonly ICouponService _coupontService;
        public GetCartsQueryHandler(AppDbContext context, OnMapping mapper, IProductService productService, ICouponService couponService)
        {
            _context = context;
            _mapper = mapper;
            _productService = productService;
            _coupontService = couponService;
        }
        public async Task<Result<IEnumerable<CartDto>>> Handle(GetCartsQuery request, CancellationToken cancellationToken)
        {
            var cartHeader = await _context.CartHeaders.FirstAsync(u=> u.UserId == request.UserId);
            if(cartHeader == null)
            {
                return await Result<IEnumerable<CartDto>>.FaildAsync(false, "Faild to return the UserId Carts");
            }
            var mappedcartHeaderResponse = await _mapper.Map<CartHeader, CartHeaderResponseDto>(cartHeader);
            var cart = new List<CartDto>
            {
                new CartDto
                {
                    CartHeaderResponse = mappedcartHeaderResponse.Data,
                }
            };
            var cartDetail = _context.CartDetails.Where(u=> u.CartHeaderId == cart.First().CartHeaderResponse.Id); 
            var mappedcartDetailsResponse = await _mapper.MapCollection<CartDetails, CartDetailsResponseDto>(cartDetail);
            cart.First().CartDetailsResponse = mappedcartDetailsResponse.Data;

            var productList = await _productService.GetAllAsync();
            foreach(var item in cart.First().CartDetailsResponse)
            {
                item.Product = productList.Data.FirstOrDefault(u => u.Id == item.ProductId);
                cart.First().CartHeaderResponse.CartTotal += (item.Count * item.Product.Price);
            };

            //Apply if there is any coupons
            if (!string.IsNullOrEmpty(cart.First().CartHeaderResponse.CouponCode))
            {
                var coupon = await _coupontService.GetCoupon(cart.First().CartHeaderResponse.CouponCode);
                if(coupon != null && cart.First().CartHeaderResponse.CartTotal > coupon.Data.MinAmount) 
                {
                    cart.First().CartHeaderResponse.CartTotal -= Convert.ToDouble(coupon.Data.DiscountAmount);
                    cart.First().CartHeaderResponse.Discount = Convert.ToDouble(coupon.Data.DiscountAmount);
                }
                else
                {
                    return await Result<IEnumerable<CartDto>>.SuccessAsync(cart, "Viewed Successfully", true);
                }
            }

            return await Result<IEnumerable<CartDto>>.SuccessAsync(cart,"Viewed Successfully", true);
        }
    }
}
