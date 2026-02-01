using SharedKernel.Domain;
using SharedKernel.Results;

namespace Product.API.Domain.ValueObjects;

/// <summary>
/// Value object representing stock quantity with business rules
/// </summary>
public sealed class StockQuantity : ValueObject
{
    public int Value { get; private set; }

    private StockQuantity(int value)
    {
        Value = value;
    }

    public static Result<StockQuantity> Create(int quantity)
    {
        if (quantity < 0)
            return Result.Failure<StockQuantity>(Error.Validation("StockQuantity.Negative", "Stock quantity cannot be negative"));

        return Result.Success(new StockQuantity(quantity));
    }

    public Result<StockQuantity> Increase(int amount)
    {
        if (amount <= 0)
            return Result.Failure<StockQuantity>(Error.Validation("StockQuantity.InvalidIncrease", "Increase amount must be positive"));

        return Create(Value + amount);
    }

    public Result<StockQuantity> Decrease(int amount)
    {
        if (amount <= 0)
            return Result.Failure<StockQuantity>(Error.Validation("StockQuantity.InvalidDecrease", "Decrease amount must be positive"));

        if (Value < amount)
            return Result.Failure<StockQuantity>(Error.Validation("StockQuantity.InsufficientStock", $"Insufficient stock. Available: {Value}, Requested: {amount}"));

        return Create(Value - amount);
    }

    public bool IsOutOfStock() => Value == 0;

    public bool IsLowStock(int threshold = 10) => Value > 0 && Value <= threshold;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator int(StockQuantity quantity) => quantity.Value;
}
