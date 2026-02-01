using SharedKernel.Domain;

namespace Product.API.Domain.Events;

/// <summary>
/// Domain event raised when a product price changes
/// </summary>
public record ProductPriceChangedDomainEvent : IDomainEvent
{
    public int ProductId { get; init; }
    public decimal OldPrice { get; init; }
    public decimal NewPrice { get; init; }
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
