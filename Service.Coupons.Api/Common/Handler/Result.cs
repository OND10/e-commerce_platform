namespace Service.Coupons.Api.Common.Handler
{

    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Data { get; set; } = default!;

        public static Task<Result> SuccessAsync(object model, string message)
        {
            return Task.FromResult(new Result { Data = model, Message = message });
        }

        public static Task<Result> SuccessAsync(object model, string message, bool success)
        {
            return Task.FromResult(new Result { Data = model, IsSuccess = success, Message = message });
        }

        public static Task<Result> SuccessAsync(string message, bool success)
        {
            return Task.FromResult(new Result { IsSuccess = success, Message = message });
        }

        public static Task<Result> FaildAsync(bool fail, string message)
        {
            return Task.FromResult(new Result { Message = message, IsSuccess = fail });
        }


    }

    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; } = default!;

        public static Task<Result<T>> SuccessAsync(T model, string message)
        {
            return Task.FromResult(new Result<T> { Data = model, Message = message });
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
