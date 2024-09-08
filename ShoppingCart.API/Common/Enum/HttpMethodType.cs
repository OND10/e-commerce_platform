namespace ShoppingCart.API.Common.Enum
{
    public class HttpMethodType
    {
        public enum ApiType
        {
            Get,
            Post,
            Put,
            Delete
        }
        public static string CouponAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string ProductAPIBase { get; set; }
    }

}
