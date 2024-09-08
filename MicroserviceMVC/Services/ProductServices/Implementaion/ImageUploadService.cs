using eCommerceWebMVC.Services.ProductServices.Interface;
using QRCoder;

namespace eCommerceWebMVC.Services.ProductServices.Implementaion
{
    public class ImageUploadService<T> : IImageUploadService<T> where T : class
    {

        private readonly IWebHostEnvironment _webHost;
        public ImageUploadService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public IFormFile FrontIamge { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

       

        public string ImageUpload(T model, IFormFile file)
        {
            string uniqueFileName = null;
            string path = "images/";
            if (file != null)
            {
                string uploadFolder = Path.Combine(_webHost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filepath = Path.Combine(uploadFolder, uniqueFileName);
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                FileStream filemode = new FileStream(filepath, FileMode.Create);

                file.CopyTo(filemode);
                filemode.Close();

            }
            return path + uniqueFileName;

        }
    }
}
