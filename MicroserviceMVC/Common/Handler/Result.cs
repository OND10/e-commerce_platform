namespace MicroserviceMVC.Common.Handler
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; } = default!;
        public int Code {  get; set; }
        public eCommerceWebMVC.Shared.HttpResponse Response { get; set; }

        public static Result<T> Success(eCommerceWebMVC.Shared.HttpResponse data, string message, bool success)
        {
            return new Result<T> { Response = data, Message = message, IsSuccess = success };
        }

        public static Result<T> Success(T model, string message, bool success)
        {
            return new Result<T> { Data = model, IsSuccess = success, Message = message };
        }

        public static Result<T> Success(string message, bool success)
        {
            return new Result<T> { IsSuccess = success, Message = message };
        }

        public static Result<T> Faild(bool fail, string message)
        {
            return new Result<T> { Message = message, IsSuccess = fail };
        }       
        public static Result<T> Faild(bool fail, string message, int code)
        {
            return new Result<T> { Message = message, IsSuccess = fail, Code = code };
        }
    }
}
