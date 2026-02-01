using SharedKernel.Domain;

namespace SharedKernel.Messaging.MessageContracts;

/// <summary>
/// Event published when a new product is created
/// </summary>
public record ProductCreatedEvent : IDomainEvent
{
    public int ProductId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Category { get; init; } = string.Empty;
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
