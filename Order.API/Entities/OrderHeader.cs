using Common.BuildingBlocks.Domain;
using Common.BuildingBlocks.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Order.API.Entities
{
    public class OrderHeader : Entity
    {
        public string? UserId { get; private set; }
        public string? CouponCode { get; private set; }
        public double Discount { get; private set; }
        public double OrderTotal { get; private set; }
        public string? Name { get; private set; }
        public string? EmailAddress { get; private set; } // Renamed to avoid confusion with Email VO if used directly
        public string? PhoneNumber { get; private set; }
        public DateTime OrderTime { get; private set; }
        public OrderState OrderState { get; private set; } // Replaced string Status with Enum
        
        // Payment Info
        public string? PaymentIntentId { get; private set; }
        public string? StripeSessionId { get; private set; }
        public string? TrackingNumber { get; private set; }

        // Navigation
        private readonly List<OrderDetails> _orderDetails = new();
        public IReadOnlyCollection<OrderDetails> OrderDetails => _orderDetails.AsReadOnly();
        
        // Value Objects (Optional: could map Name/Address/Email to VOs)
        // public Address BillingAddress { get; private set; } 

        // Constructor for EF Core
        private OrderHeader() { }

        // Factory Method
        public static OrderHeader Create(string userId, string name, string email, string phone, double orderTotal)
        {
            return new OrderHeader
            {
                UserId = userId,
                Name = name,
                EmailAddress = email,
                PhoneNumber = phone,
                OrderTotal = orderTotal,
                OrderTime = DateTime.UtcNow,
                OrderState = OrderState.Created
            };
        }

        public void AddLineItem(int productId, string productName, double price, int count)
        {
            if (count <= 0) throw new ArgumentException("Count must be greater than zero");
            
            _orderDetails.Add(new OrderDetails(productId, productName, price, count));
        }

        public void SetPaymentIntent(string intentId, string sessionId)
        {
            PaymentIntentId = intentId;
            StripeSessionId = sessionId;
        }

        public void SetTracking(string trackingNumber)
        {
            TrackingNumber = trackingNumber;
        }

        internal void SetState(OrderState newState)
        {
            OrderState = newState;
        }
    }
}
