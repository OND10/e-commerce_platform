using SharedKernel.Domain;
using Order.API.Entities;

namespace Order.API.Features.Orders.Events
{
    public record OrderCancelledDomainEvent(OrderHeader Order) : IDomainEvent;
}
