namespace MicroserviceMVC.Common.Enum
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
        public static string OrderAPIBase { get; set; }
        public static string CartAPIBase {  get; set; }
        public const string RoleAdmin = "Admin";
        public const string RoleCustomer = "Customer";
        public const string TokenCookie = "JWTToken";

    }

}
