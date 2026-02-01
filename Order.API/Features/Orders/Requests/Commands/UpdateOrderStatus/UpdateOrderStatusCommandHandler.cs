using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;
using Order.API.Entities;
using Order.API.DataBase;
using Stripe;
using Order.API.Common.Enum; // Leaving generic Enum usage if needed for comparison, but relying on StateMachine

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
            var orderHeader = await _context.OrderHeaders.FirstOrDefaultAsync(o => o.Id == request.orderId, cancellationToken);

            if (orderHeader is null)
                return Result.Failure<bool>("Order not found");

            if (request.newStatus == StatusEnum.Status_Cancelled)
            {
                var stateMachine = new OrderStateMachine(orderHeader, () => true);
                
                if (stateMachine.CanFire(OrderTrigger.Cancel))
                {
                    stateMachine.Fire(OrderTrigger.Cancel);
                    
                    if (!string.IsNullOrEmpty(orderHeader.PaymentIntentId))
                    {
                        var options = new RefundCreateOptions
                        {
                            Reason = RefundReasons.RequestedByCustomer,
                            PaymentIntent = orderHeader.PaymentIntentId,
                        };
                        var service = new RefundService();
                        await service.CreateAsync(options, cancellationToken: cancellationToken);
                    }
                }
                else
                {
                     return Result.Failure<bool>($"Cannot cancel order in state {orderHeader.OrderState}");
                }
            }
            // Add other status handling if needed

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(true);
        }
    }
}
