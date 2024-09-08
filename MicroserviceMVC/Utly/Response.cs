using MicroserviceMVC;

namespace MicroserviceMVC
{
    public class Response<T> : IResponse<T>
    {
        public string Status { get; set; } = "";
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public object DataPre { get; set; }

        public T Data { get; set; }

        public static async Task<IResponse> SuccessAsync(T model, string message)
        {
            return await Task.FromResult<Response<T>>(new Response<T> { Data= model,Message=message,IsSuccess=true});
        }
        //public static async Task<IResponse> SuccessAsync(T model, string message)
        //{
        //    return await Task.FromResult<Response<T>>(new Response<T> { Data = model, Message = message, IsSuccess = true });
        //}

        public static IResponse Success()
        {
            return new Response<T> { IsSuccess=true };
        }
    }

    //public class Response<Coupon> : IResponse<Coupon>
    //{
    //    public Coupon Data { get; set; }
    //    public string Status { get; set; } = "";
    //    public string Message { get; set; } = string.Empty;
    //    public bool IsSuccess { get; set; }
    //    public object DataPre { get; set; }

    //    public static async Task<Response> Fail()
    //    {
    //        return await Task.FromResult<Response>(new Response { IsSuccess = false });
    //    }

    //    public static async Task<Response> Fail(string message)
    //    {
    //        return await Task.FromResult<Response>(new Response { IsSuccess = false, Message=message });
    //    }

    //    public static async Task<Response<Coupon>> SuccessAsync(Task<Model.Coupon> findcode, Coupon Data)
    //    {
    //        return await Task.FromResult<Response<Coupon>>(new Response<Coupon> { IsSuccess = true, Data = Data });
    //    }

    //    public static Response<Coupon> Success(Coupon Data ,string message)
    //    {
    //        return new Response<Coupon> { Data = Data ,Message = message };
    //    }

    //    public static async Task<Response<Coupon>> SuccessAsync(Coupon Data, string message)
    //    {
    //        return await Task.FromResult<Response<Coupon>>(new Response<Coupon> { IsSuccess = true, Data = Data, Message = message });
    //    }

    //    //public static async Task<Response> SuccessAsync(Coupon Data,string message)
    //    //{
    //    //    return await Task.FromResult<Response>(new Response { DataPre = Data, Message = message });
    //    //}

    //}
}
