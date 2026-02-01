using SharedKernel.Domain;
using SharedKernel.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using Order.API.Features.Orders.Events;

namespace Order.API.Entities
{
    public class OrderHeader : AggregateRoot
    {
        public string? UserId { get; private set; }
        public string? CouponCode { get; private set; }
        public double Discount { get; private set; }
        public double OrderTotal { get; private set; }
        public string? Name { get; private set; }
        public string? EmailAddress { get; private set; }
        public string? PhoneNumber { get; private set; }
        public DateTime OrderTime { get; private set; }
        public OrderState OrderState { get; private set; }
        
        // Payment Info
        public string? PaymentIntentId { get; private set; }
        public string? StripeSessionId { get; private set; }
        public string? TrackingNumber { get; private set; }

        // Navigation
        private readonly List<OrderDetails> _orderDetails = new();
        public IReadOnlyCollection<OrderDetails> OrderDetails => _orderDetails.AsReadOnly();
        
        // Constructor for EF Core
        private OrderHeader() { }

        // Factory Method
        public static OrderHeader Create(string userId, string name, string email, string phone, double orderTotal)
        {
            var order = new OrderHeader
            {
                UserId = userId,
                Name = name,
                EmailAddress = email,
                PhoneNumber = phone,
                OrderTotal = orderTotal,
                OrderTime = DateTime.UtcNow,
                OrderState = OrderState.Created
            };
            
            order.AddDomainEvent(new OrderCreatedDomainEvent(order));
            
            return order;
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
