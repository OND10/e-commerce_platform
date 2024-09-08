namespace MicroserviceMVC.Common.Handler
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; } = default!;
        public eCommerceWebMVC.Shared.HttpResponse Response { get; set; }

        public static Task<Result<T>> SuccessAsync(eCommerceWebMVC.Shared.HttpResponse data, string message, bool success)
        {
            return Task.FromResult(new Result<T> { Response = data, Message = message, IsSuccess = success });
        }

        public static Task<Result<T>> SuccessAsync(T model, string message, bool success)
        {
            return Task.FromResult(new Result<T> { Data = model, IsSuccess = success, Message = message });
        }

        public static Task<Result<T>> SuccessAsync(string message, bool success)
        {
            return Task.FromResult(new Result<T> { IsSuccess = success, Message = message });
        }

        public static Task<Result<T>> FaildAsync(bool fail, string message)
        {
            return Task.FromResult(new Result<T> { Message = message, IsSuccess = fail });
        }
    }
}
