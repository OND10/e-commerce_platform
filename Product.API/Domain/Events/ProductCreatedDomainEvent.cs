using SharedKernels.Domain;

namespace Product.API.Domain.Events;

/// <summary>
/// Domain event raised when a new product is created
/// </summary>
public record ProductCreatedDomainEvent : IDomainEvent
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Category { get; init; } = string.Empty;
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
