using Microsoft.EntityFrameworkCore;
using Product.API.Domain.ValueObjects;
using SharedKernel.Results;
using System.Reflection;

namespace Product.API.DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Apply all configurations from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // Seed Product data
            // Note: For Owned Types (Price/Money), we must seed them separately using OwnsOne.HasData
            // For Value Objects with Conversion (Name, Stock), we can pass the Value Object instance inside the anonymous object,
            // or if HasConversion is set, EF Core should handle it.
            // Since ProductName and StockQuantity factories return Result<T>, we use .Value.

            modelBuilder.Entity<Entities.Product>().HasData(
                new 
                { 
                    Id = 1, 
                    Name = ProductName.Create("IPhone 15 Pro Max").Value, 
                    Description = "The latest iPhone with titanium design.", 
                    Stock = StockQuantity.Create(10).Value, 
                    Category = "Phones", 
                    ImageUrl = "https://placehold.co/603x403" 
                },
                new 
                { 
                    Id = 2, 
                    Name = ProductName.Create("Samsung Galaxy S24 Ultra").Value, 
                    Description = "AI-powered smartphone.", 
                    Stock = StockQuantity.Create(15).Value, 
                    Category = "Phones", 
                    ImageUrl = "https://placehold.co/603x403" 
                },
                new 
                { 
                    Id = 3, 
                    Name = ProductName.Create("MacBook Pro 14").Value, 
                    Description = "M3 Pro chip laptop.", 
                    Stock = StockQuantity.Create(5).Value, 
                    Category = "Laptops", 
                    ImageUrl = "https://placehold.co/603x403" 
                },
                new 
                { 
                    Id = 4, 
                    Name = ProductName.Create("Sony WH-1000XM5").Value, 
                    Description = "Noise cancelling headphones.", 
                    Stock = StockQuantity.Create(20).Value, 
                    Category = "Audio", 
                    ImageUrl = "https://placehold.co/603x403" 
                },
                new 
                { 
                    Id = 5, 
                    Name = ProductName.Create("iPad Air 5").Value, 
                    Description = "M1 chip tablet.", 
                    Stock = StockQuantity.Create(8).Value, 
                    Category = "Tablets", 
                    ImageUrl = "https://placehold.co/603x403" 
                }
            );

            // Seed Owned Type (Money) separately
            modelBuilder.Entity<Entities.Product>().OwnsOne(p => p.Price).HasData(
                new { ProductId = 1, Amount = 1199m, Currency = "USD" },
                new { ProductId = 2, Amount = 1299m, Currency = "USD" },
                new { ProductId = 3, Amount = 1999m, Currency = "USD" },
                new { ProductId = 4, Amount = 348m, Currency = "USD" },
                new { ProductId = 5, Amount = 599m, Currency = "USD" }
            );
        }

        public DbSet<Entities.Product> Products { get; set; }
    }
}
