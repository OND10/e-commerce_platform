namespace eCommerceWebMVC.Services.ProductServices.Interface
{
    public interface IImageUploadService<T> where T : class
    {
        public IFormFile FrontIamge { get; set; }
        public string ImageUpload(T model, IFormFile file);
    }
}
