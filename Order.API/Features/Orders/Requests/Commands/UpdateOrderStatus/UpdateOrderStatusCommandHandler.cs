using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.API.Common.Enum;
using Order.API.Common.Handler;
using Order.API.DataBase;
using Order.API.Features.Orders.Dtos.Response;
using Stripe;

namespace Order.API.Features.Orders.Requests.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Result<bool>>
    {
        private readonly AppDbContext _context;

        public UpdateOrderStatusCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var orderHeader = await _context.OrderHeaders.FirstAsync(o => o.Id == request.orderId);

            if (orderHeader is not null)
            {
                if (request.newStatus == StatusEnum.Status_Cancelled)
                {
                    // giving refund
                    var options = new RefundCreateOptions
                    {
                        Reason = RefundReasons.RequestedByCustomer,
                        PaymentIntent = orderHeader.PaymentIntentId,

                    };

                    var service = new RefundService();
                    Refund refund = await service.CreateAsync(options);
                }

                orderHeader.Status = request.newStatus;
                await _context.SaveChangesAsync();

                return await Result<bool>.SuccessAsync(true, "OrderStatus is updated Successfully", true);
            }

            return await Result<bool>.FaildAsync(false, "OrderStatus is not updated");
        }
    }
}
