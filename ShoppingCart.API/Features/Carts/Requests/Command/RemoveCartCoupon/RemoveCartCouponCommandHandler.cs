using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Common.Handler;
using ShoppingCart.API.DataBase;
using ShoppingCart.API.Features.Carts.Requests.Command.ApplyCartCoupon;

namespace ShoppingCart.API.Features.Carts.Requests.Command.RemoveCartCoupon
{
    public class RemoveCartCouponCommandHandler : IRequestHandler<RemoveCartCouponCommand, Result<bool>>
    {
        private readonly AppDbContext _context;
        public RemoveCartCouponCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<bool>> Handle(RemoveCartCouponCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cartHeader = await _context.CartHeaders.FirstAsync(u => u.UserId == request.CartHeaderResponse.UserId);
                cartHeader.CouponCode = "";
                _context.Update(cartHeader);
                await _context.SaveChangesAsync();
                return await Result<bool>.SuccessAsync(true, "Applied Successfully", true);
            }
            catch
            {
                return await Result<bool>.FaildAsync(false, "Faild in applying Coupon");
            }
        }
    }
}
