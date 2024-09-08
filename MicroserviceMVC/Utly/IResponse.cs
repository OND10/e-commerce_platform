namespace MicroserviceMVC
{
    public interface IResponse
    {

        public string Status { get; set; }

        public string Message { get; set; }

        public bool IsSuccess { get; set; }

        public object DataPre { get; set; }

    }

    public interface IResponse<out T> : IResponse
    {
       public T Data { get; }
    }
}
