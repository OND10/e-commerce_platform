using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Common.Handler;
using ShoppingCart.API.DataBase;

namespace ShoppingCart.API.Features.Carts.Requests.Command.ApplyCartCoupon
{
    public class ApplyCartCouponCommandHandler : IRequestHandler<ApplyCartCouponCommand, Result<bool>>
    {
        private readonly AppDbContext _context;
        public ApplyCartCouponCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<bool>> Handle(ApplyCartCouponCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cartHeader =await _context.CartHeaders.FirstAsync(u=> u.UserId == request.CartHeaderResponse.UserId);
                cartHeader.CouponCode = request.CartHeaderResponse.CouponCode;
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
