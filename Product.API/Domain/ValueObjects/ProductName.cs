using SharedKernel.Domain;
using SharedKernel.Results;

namespace Product.API.Domain.ValueObjects;

/// <summary>
/// Value object representing a product name with validation
/// </summary>
public sealed class ProductName : ValueObject
{
    public string Value { get; private set; }

    private ProductName(string value)
    {
        Value = value;
    }

    public static Result<ProductName> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<ProductName>(Error.Validation("ProductName.Empty", "Product name cannot be empty"));

        if (name.Length > 200)
            return Result.Failure<ProductName>(Error.Validation("ProductName.TooLong", "Product name cannot exceed 200 characters"));

        if (name.Length < 3)
            return Result.Failure<ProductName>(Error.Validation("ProductName.TooShort", "Product name must be at least 3 characters"));

        return Result.Success(new ProductName(name.Trim()));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(ProductName productName) => productName.Value;
}
