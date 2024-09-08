using System.ComponentModel.DataAnnotations;

namespace Product.API.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberofProduct { get; set; }
        public string Category { get; set; }
        [Range(1,1000)]
        public double Price { get; set; }
        public string ImageUrl {  get; set; }
    }
}
