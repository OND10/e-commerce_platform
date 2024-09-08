using static Order.API.Common.Enum.HttpMethodType;

namespace eCommerceWebMVC.Shared
{
    public class HttpRequest
    {
        public ApiType apiType { get; set; } = ApiType.Get;

        public string Url { get; set; } = string.Empty;

        public object Data { get; set; } = null!;

        public string AccessToken { get; set; } = string.Empty;
    }
}
