using SharedKernel.Domain;

namespace SharedKernel.Messaging.MessageContracts;

/// <summary>
/// Event published when an order is placed
/// </summary>
public record OrderPlacedEvent : IDomainEvent
{
    public int OrderId { get; init; }
    public int UserId { get; init; }
    public decimal TotalAmount { get; init; }
    public List<OrderItemDto> Items { get; init; } = new();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public record OrderItemDto
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal Price { get; init; }
}
