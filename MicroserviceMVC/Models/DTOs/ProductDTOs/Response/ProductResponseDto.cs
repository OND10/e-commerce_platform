using System.ComponentModel.DataAnnotations;

namespace eCommerceWebMVC.Models.DTOs.ProductDTOs.Response
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0,100)]
        public int NumberofProduct { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile FrontIamge { get; set; }
        
    }
}
