namespace ShoppingCart.API.Shared
{
    public class HttpResponse
    {
        public object? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
