using SharedKernel.Domain;
using SharedKernel.Results;
using Product.API.Domain.ValueObjects;
using Product.API.Domain.Events;

namespace Product.API.Entities
{
    /// <summary>
    /// Product aggregate root with rich domain model
    /// </summary>
    public class Product : AggregateRoot
    {
        // Private parameterless constructor for EF Core
        private Product() { }

        // Private constructor for creating valid products
        private Product(ProductName name, string description, Money price, StockQuantity stock, string category, string imageUrl)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            Category = category;
            ImageUrl = imageUrl;

            // Raise domain event
            Raise(new ProductCreatedDomainEvent
            {
                ProductId = Id,
                ProductName = name.Value,
                Price = price.Amount,
                Category = category
            });
        }

        // Properties with private setters
        public ProductName Name { get; private set; } = null!;
        public string Description { get; private set; } = string.Empty;
        public Money Price { get; private set; } = null!;
        public StockQuantity Stock { get; private set; } = null!;
        public string Category { get; private set; } = string.Empty;
        public string ImageUrl { get; private set; } = string.Empty;

        /// <summary>
        /// Factory method to create a new product with validation
        /// </summary>
        public static Result<Product> Create(
            string name,
            string description,
            decimal price,
            int stock,
            string category,
            string imageUrl = "")
        {
            // Validate and create value objects
            var productNameResult = ProductName.Create(name);
            if (productNameResult.IsFailure)
                return Result.Failure<Product>(productNameResult.Error);

            var moneyResult = Money.Create(price);
            if (moneyResult.IsFailure)
                return Result.Failure<Product>(moneyResult.Error);

            var stockResult = StockQuantity.Create(stock);
            if (stockResult.IsFailure)
                return Result.Failure<Product>(stockResult.Error);

            // Additional validation
            if (string.IsNullOrWhiteSpace(category))
                return Result.Failure<Product>(Error.Validation("Product.Category", "Category is required"));

            return Result.Success(new Product(
                productNameResult.Value,
                description ?? string.Empty,
                moneyResult.Value,
                stockResult.Value,
                category,
                imageUrl ?? string.Empty
            ));
        }

        /// <summary>
        /// Updates the product price
        /// </summary>
        public Result UpdatePrice(decimal newPrice)
        {
            var moneyResult = Money.Create(newPrice);
            if (moneyResult.IsFailure)
                return Result.Failure(moneyResult.Error);

            var oldPrice = Price.Amount;
            Price = moneyResult.Value;

            Raise(new ProductPriceChangedDomainEvent
            {
                ProductId = Id,
                OldPrice = oldPrice,
                NewPrice = newPrice
            });

            return Result.Success();
        }

        /// <summary>
        /// Decreases stock quantity (e.g., when an order is placed)
        /// </summary>
        public Result DecreaseStock(int quantity)
        {
            var decreaseResult = Stock.Decrease(quantity);
            if (decreaseResult.IsFailure)
                return Result.Failure(decreaseResult.Error);

            Stock = decreaseResult.Value;

            if (Stock.IsOutOfStock())
            {
                Raise(new ProductOutOfStockDomainEvent
                {
                    ProductId = Id,
                    ProductName = Name.Value
                });
            }

            return Result.Success();
        }

        /// <summary>
        /// Increases stock quantity (e.g., when inventory is replenished)
        /// </summary>
        public Result IncreaseStock(int quantity)
        {
            var previousStock = Stock.Value;
            var increaseResult = Stock.Increase(quantity);
            if (increaseResult.IsFailure)
                return Result.Failure(increaseResult.Error);

            Stock = increaseResult.Value;

            Raise(new ProductStockReplenishedDomainEvent
            {
                ProductId = Id,
                PreviousStock = previousStock,
                NewStock = Stock.Value
            });

            return Result.Success();
        }

        /// <summary>
        /// Updates product details
        /// </summary>
        public Result Update(string name, string description, decimal price, int stock, string category, string imageUrl)
        {
            // Validate name
            var nameResult = ProductName.Create(name);
            if (nameResult.IsFailure)
                return Result.Failure(nameResult.Error);

            // Validate price
            var priceResult = Money.Create(price);
            if (priceResult.IsFailure)
                return Result.Failure(priceResult.Error);

            // Validate stock
            var stockResult = StockQuantity.Create(stock);
            if (stockResult.IsFailure)
                return Result.Failure(stockResult.Error);

            // Validate category
            if (string.IsNullOrWhiteSpace(category))
                return Result.Failure(Error.Validation("Product.Category", "Category is required"));

            // Update properties
            Name = nameResult.Value;
            Description = description ?? string.Empty;
            
            // Check if price changed
            if (Price.Amount != price)
            {
                var oldPrice = Price.Amount;
                Price = priceResult.Value;
                Raise(new ProductPriceChangedDomainEvent
                {
                    ProductId = Id,
                    OldPrice = oldPrice,
                    NewPrice = price
                });
            }
            else
            {
                Price = priceResult.Value;
            }

            Stock = stockResult.Value;
            Category = category;
            ImageUrl = imageUrl ?? string.Empty;

            return Result.Success();
        }

        /// <summary>
        /// Checks if the product is available for purchase
        /// </summary>
        public bool IsAvailable() => !Stock.IsOutOfStock();

        /// <summary>
        /// Checks if the product has low stock
        /// </summary>
        public bool HasLowStock(int threshold = 10) => Stock.IsLowStock(threshold);
    }
}
