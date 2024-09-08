using Microsoft.EntityFrameworkCore;

namespace Product.API.DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Entities.Product>().HasData(new Entities.Product
            {
                Id = 1,
                Name = "Product1",
                Price = 15,
                Description = "Description about products",
                ImageUrl = "https://placehold.co/603x403",
                Category = "Phones"
            });

            modelBuilder.Entity<Entities.Product>().HasData(new Entities.Product
            {
                Id = 2,
                Name = "Product2",
                Price = 45,
                Description = "Description about products",
                ImageUrl = "https://placehold.co/603x403",
                Category = "Phones"
            });

            modelBuilder.Entity<Entities.Product>().HasData(new Entities.Product
            {
                Id = 3,
                Name = "Product3",
                Price = 60,
                Description = "Description about products",
                ImageUrl = "https://placehold.co/603x403",
                Category = "Phones"
            });

            modelBuilder.Entity<Entities.Product>().HasData(new Entities.Product
            {
                Id = 4,
                Name = "Product4",
                Price = 136,
                Description = "Description about products",
                ImageUrl = "https://placehold.co/603x403",
                Category = "Phones"
            });

            modelBuilder.Entity<Entities.Product>().HasData(new Entities.Product
            {
                Id = 5,
                Name = "Product5",
                Price = 6,
                Description = "Description about products",
                ImageUrl = "https://placehold.co/603x403",
                Category = "Phones"
            });

            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<Entities.Product> Products { get; set; }
    }

}
