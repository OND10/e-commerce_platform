using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernels.Results;
using Order.API.Entities;
using Order.API.DataBase;
using Stripe;
using Order.API.Common.Enum;
using SharedKernels.Abstractions.Messaging; // Leaving generic Enum usage if needed for comparison, but relying on StateMachine

namespace Order.API.Features.Orders.Requests.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : ICommandHandler<UpdateOrderStatusCommand, bool>
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
                return Result.Failure<bool>(OrderNotFoundError);

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
                     Error OrderCanotBeCanceled = new("500", $"Cannot cancel order in state {orderHeader.OrderState}", SharedKernels.ErrorType.Failure);
                     return Result.Failure<bool>(OrderCanotBeCanceled);
                }
            }
            // Add other status handling if needed

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(true);
        }

        private Error OrderNotFoundError => new("401", "Order not found", SharedKernels.ErrorType.NotFound);
        
    }
}
