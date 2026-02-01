using MassTransit;
using SharedKernels.Messaging.MessageContracts;
using Product.API.Features.Products.Repository.Interface;
using Microsoft.Extensions.Logging;

namespace Product.API.Features.Products.Consumers;

/// <summary>
/// Consumer for OrderPlacedEvent to decrease product stock
/// </summary>
public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<OrderPlacedConsumer> _logger;

    public OrderPlacedConsumer(IProductRepository productRepository, ILogger<OrderPlacedConsumer> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
    {
        _logger.LogInformation("Consuming OrderPlacedEvent for OrderId: {OrderId}", context.Message.OrderId);

        foreach (var item in context.Message.Items)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    _logger.LogWarning("Product {ProductId} not found while processing Order {OrderId}", item.ProductId, context.Message.OrderId);
                    continue;
                }

                var result = product.DecreaseStock(item.Quantity);
                
                if (result.IsFailure)
                {
                    _logger.LogError("Failed to decrease stock for Product {ProductId}: {Error}", item.ProductId, result.Error.Description);
                    // In a real scenario, we might want to compensate or publish an OrderFailedEvent
                    // For now, we log and continue (or could throw to retry consumer)
                    throw new InvalidOperationException($"Insufficient stock for Product {item.ProductId}");
                }

                await _productRepository.UpdateAsync(product);
                _logger.LogInformation("Decreased stock for Product {ProductId} by {Quantity}. New stock: {Stock}", 
                    item.ProductId, item.Quantity, product.Stock.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing product {ProductId} for Order {OrderId}", item.ProductId, context.Message.OrderId);
                throw; // Retry via MassTransit
            }
        }
    }
}
