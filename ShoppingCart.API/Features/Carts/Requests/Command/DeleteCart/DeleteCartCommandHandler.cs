using MediatR;
using Microsoft.EntityFrameworkCore;
using OnMapper;
using ShoppingCart.API.Common.Handler;
using ShoppingCart.API.DataBase;
using ShoppingCart.API.Entities;
using ShoppingCart.API.Features.Carts.Requests.Command.AddCart;
using ShoppingCart.API.Features.DTOs.CartDetailsDTOs.Response;
using ShoppingCart.API.Features.DTOs.CartDTOs;
using ShoppingCart.API.Features.DTOs.CartHeaderDTOs.Response;

namespace ShoppingCart.API.Features.Carts.Requests.Command.DeleteCart
{
    public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, Result<bool>>
    {
        private readonly AppDbContext _context;
        private readonly OnMapping _mapper;
        public DeleteCartCommandHandler(AppDbContext context, OnMapping mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<bool>> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cartDetails = _context.CartDetails.First(u => u.Id == request.cartDetailsId);
                int totalCountofCartItem = _context.CartDetails.Where(u=> u.CartHeaderId == cartDetails.CartHeaderId).Count();
                _context.CartDetails.Remove(cartDetails);
                if(totalCountofCartItem == 1)
                {
                    var cartHeadertoGetReomved = await _context.CartHeaders.FirstOrDefaultAsync(u=> u.Id == cartDetails.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeadertoGetReomved);
                }

                await _context.SaveChangesAsync();
                return await Result<bool>.SuccessAsync(true, "Deleted Successfully", true);
            }
            catch
            {
                return await Result<bool>.FaildAsync(false, "Not Deleted");
            }
        }
    }
}
