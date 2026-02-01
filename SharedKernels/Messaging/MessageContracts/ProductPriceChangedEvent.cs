using SharedKernels.Domain;

namespace SharedKernels.Messaging.MessageContracts;

/// <summary>
/// Event published when a product price changes
/// </summary>
public record ProductPriceChangedEvent : IDomainEvent
{
    public int ProductId { get; init; }
    public decimal OldPrice { get; init; }
    public decimal NewPrice { get; init; }
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
