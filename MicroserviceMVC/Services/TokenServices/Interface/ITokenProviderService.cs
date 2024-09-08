namespace eCommerceWebMVC.Services.TokenServices.Interface
{
    public interface ITokenProviderService
    {
        public void ClearToken();
        public string GetToken();
        public void SetToken(string token);
    }
}
