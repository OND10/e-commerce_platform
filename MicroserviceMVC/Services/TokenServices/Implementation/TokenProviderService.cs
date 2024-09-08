using eCommerceWebMVC.Common.Enum;
using eCommerceWebMVC.Services.TokenServices.Interface;

namespace eCommerceWebMVC.Services.TokenServices.Implementation
{
    public class TokenProviderService : ITokenProviderService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProviderService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(TokenProvider.TokenCookie);
        }

        public string GetToken()
        {
            string? token = null;
            bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(TokenProvider.TokenCookie, out token);
            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(TokenProvider.TokenCookie, token);
        }
    }
}
