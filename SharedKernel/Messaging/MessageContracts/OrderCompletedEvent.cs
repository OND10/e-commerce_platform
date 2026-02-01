using SharedKernel.Domain;

namespace SharedKernel.Messaging.MessageContracts;

/// <summary>
/// Event published when an order is completed
/// </summary>
public record OrderCompletedEvent : IDomainEvent
{
    public int OrderId { get; init; }
    public int UserId { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime CompletedAt { get; init; }
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
