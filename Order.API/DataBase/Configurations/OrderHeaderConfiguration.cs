using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.API.Entities;

namespace Order.API.DataBase.Configurations
{
    public class OrderHeaderConfiguration : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.OrderTotal).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.EmailAddress).IsRequired();
            builder.Property(x => x.PhoneNumber).IsRequired();

            // Enums as strings or int? Default is int. Let's stick to default or string conversion if needed.
            // User requested fluent api.
            builder.Property(x => x.OrderState)
                .HasConversion<string>(); 

            // Owned types if we had them (Address, etc)
            // builder.OwnsOne(o => o.ShippingAddress, a => { ... });

            // Navigation
            builder.HasMany(x => x.OrderDetails)
                   .WithOne(x => x.OrderHeader)
                   .HasForeignKey(x => x.OrderHeaderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
