using Microsoft.AspNetCore.SignalR;

namespace eCommerceWebMVC.Services.SignalServices
{
    public class ProductHub : Hub
    {
        public async Task SendProductPriceUpdate(int productId, int numberOfProduct)
        {
            // Calculate new price based on numberOfProduct
            int newPrice = CalculateNewPrice(numberOfProduct);
            await Clients.Caller.SendAsync("ReceivePriceUpdate", productId, newPrice);
        }

        private int CalculateNewPrice(int numberOfProduct)
        {
            // Implement your logic to calculate the new price based on the number of products
            return numberOfProduct * 10; // Example logic
        }
    }
}
