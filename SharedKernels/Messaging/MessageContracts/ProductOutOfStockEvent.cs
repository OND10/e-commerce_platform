using SharedKernels.Domain;

namespace SharedKernels.Messaging.MessageContracts;

/// <summary>
/// Event published when a product goes out of stock
/// </summary>
public record ProductOutOfStockEvent : IDomainEvent
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
