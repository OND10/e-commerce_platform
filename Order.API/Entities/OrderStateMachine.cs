using SharedKernel.StateManagement;
using System;
using System.Collections.Generic;

namespace Order.API.Entities
{
    public sealed class OrderStateMachine
    {
        private readonly OrderHeader _order;
        private readonly StateMachine<OrderState, OrderTrigger> _stateMachine;

        public OrderStateMachine(OrderHeader order, Func<bool> paymentCollectedGuard)
        {
            _order = order;
            _stateMachine = new StateMachine<OrderState, OrderTrigger>()
                .Permit(OrderState.Created, OrderState.Paid, OrderTrigger.Pay, guard: paymentCollectedGuard)
                .Permit(OrderState.Created, OrderState.Cancelled, OrderTrigger.Cancel)
                .Permit(OrderState.Paid, OrderState.Packed, OrderTrigger.Pack)
                .Permit(OrderState.Paid, OrderState.Cancelled, OrderTrigger.Cancel)
                .Permit(OrderState.Packed, OrderState.Shipped, OrderTrigger.Ship, onTransition: () =>
                {
                    // Logic moved to Domain Method or kept here if purely side-effect free on other aggregates
                    // But here we are setting TrackingNumber on the order itself
                    if (string.IsNullOrWhiteSpace(_order.TrackingNumber))
                    {
                        _order.SetTracking($"TRK-{_order.Id.ToString().PadLeft(8, '0')}");
                    }
                })
                .Permit(OrderState.Shipped, OrderState.Delivered, OrderTrigger.Deliver);
        }

        public bool CanFire(OrderTrigger trigger)
        {
            return _stateMachine.CanFire(_order.OrderState, trigger);
        }
        
        public void Fire(OrderTrigger trigger)
        {
            var newState = _stateMachine.Fire(_order.OrderState, trigger);
            _order.SetState(newState);    
        }
        
        public IEnumerable<(OrderTrigger triggers,  OrderState toStates)> GetPermittedTriggers()
        {
            return _stateMachine.GetPermittedTriggers(_order.OrderState);
        }
    }
}
