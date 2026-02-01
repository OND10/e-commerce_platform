using SharedKernels.Domain;

namespace Product.API.Domain.Events;

/// <summary>
/// Domain event raised when product stock is replenished
/// </summary>
public record ProductStockReplenishedDomainEvent : IDomainEvent
{
    public int ProductId { get; init; }
    public int PreviousStock { get; init; }
    public int NewStock { get; init; }
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
