using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.API.Entities;

namespace Order.API.DataBase.Configurations
{
    public class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetails>
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductName).IsRequired();
            builder.Property(x => x.Count).IsRequired();
            builder.Property(x => x.Price).IsRequired();

            // Ignore calculated or unmapped properties
            builder.Ignore(x => x.Product);
        }
    }
}
