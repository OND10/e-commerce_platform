namespace Order.API.Entities
{
    public enum OrderState
    {
        Created,
        Paid,
        Packed,
        Shipped,
        Delivered,
        Cancelled
    }

    public enum OrderTrigger
    {
        Pay,
        Pack,
        Ship,
        Deliver,
        Cancel
    }
}
