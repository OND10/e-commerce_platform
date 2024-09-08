namespace eCommerceWebMVC.Models.DTOs.ProductDTOs.Request
{
    public class ProductRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberofProduct { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile FrontIamge { get; set; }
    }
}
