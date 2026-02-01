using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.API.Domain.ValueObjects;

namespace Product.API.DataBase.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Entities.Product>
    {
        public void Configure(EntityTypeBuilder<Entities.Product> builder)
        {
            builder.HasKey(p => p.Id);

            // Configure ProductName value object
            builder.Property(p => p.Name)
                .HasConversion(
                    name => name.Value,
                    value => ProductName.Create(value).Value)
                .HasMaxLength(200)
                .IsRequired();

            // Configure StockQuantity value object
            builder.Property(p => p.Stock)
                .HasConversion(
                    stock => stock.Value,
                    value => StockQuantity.Create(value).Value)
                .IsRequired();

            // Configure Money value object with OwnsOne for cleaner table structure if preferred, 
            // but here we are mapping to flat columns as per previous schema to minimize migration friction 
            // or we use Owned Entity. Let's use Owned Types for Money to allow multi-currency in future effectively.
            // BUT, to keep it simple and match existing Price column (double) -> we need to be careful.
            // Existing Price was double. New Money is decimal.
            // Let's use HasConversion for now to map Money Amount to decimal column, 
            // and maybe ignore Currency or store it in a separate column if we want.
            // Given the existing schema had just 'Price' (double), let's map Amount to 'Price' column 
            // and assume default currency for now to avoid breaking changes if possible, 
            // OR we go full DDD and use OwnsOne.
            // User asked to "Update AppDbContext with value object configurations".
            // Transforming existing double Price to Money (decimal + Currency) is a breaking change.
            // Let's use OwnsOne but map Amount to "Price" column name.
            
            builder.OwnsOne(p => p.Price, priceBuilder =>
            {
                priceBuilder.Property(m => m.Amount)
                    .HasColumnName("Price")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                priceBuilder.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .HasDefaultValue("USD")
                    .IsRequired();
            });

            builder.Property(p => p.Description)
                .IsRequired(false);

            builder.Property(p => p.Category)
                .IsRequired();

            builder.Property(p => p.ImageUrl)
                .IsRequired(false);
            
            // Ignore the domain events list
            builder.Ignore(p => p.DomainEvents);
        }
    }
}
