using MicroserviceMVC.Common.Handler;

namespace MicroserviceMVC.Service.WebServices.Interface
{
    public interface IBaseService
    {
        Task<Result<eCommerceWebMVC.Shared.HttpResponse>> SendAsync(eCommerceWebMVC.Shared.HttpRequest request, bool withBearer = true);
    }
}
