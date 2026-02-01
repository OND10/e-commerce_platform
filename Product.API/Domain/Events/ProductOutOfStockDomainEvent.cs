using SharedKernels.Domain;

namespace Product.API.Domain.Events;

/// <summary>
/// Domain event raised when a product goes out of stock
/// </summary>
public record ProductOutOfStockDomainEvent : IDomainEvent
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
