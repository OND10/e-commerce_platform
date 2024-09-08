using MicroserviceMVC.Common.Enum;

namespace eCommerceWebMVC.Shared
{
    public static class ApiTypeExtensions
    {
        public static HttpMethod ToHttpMethod(this HttpMethodType.ApiType apiType)
        {
            return apiType switch
            {
                HttpMethodType.ApiType.Get => HttpMethod.Get,
                HttpMethodType.ApiType.Post => HttpMethod.Post,
                HttpMethodType.ApiType.Put => HttpMethod.Put,
                HttpMethodType.ApiType.Delete => HttpMethod.Delete,
                _ => throw new ArgumentOutOfRangeException(nameof(apiType), apiType, null)
            };
        }
    }
}
