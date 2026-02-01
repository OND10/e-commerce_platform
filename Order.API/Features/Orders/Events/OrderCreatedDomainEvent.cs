using SharedKernels.Domain;
using Order.API.Entities;

namespace Order.API.Features.Orders.Events
{
    public record OrderCreatedDomainEvent(OrderHeader Order) : IDomainEvent;
}
