using Microsoft.EntityFrameworkCore;
using Service.Coupons.Api.Model;

namespace Service.Coupons.Api.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //Seeding to the Coupon Entity with two records
            //modelBuilder.Entity<Service.Coupons.Api.Model.Coupon>().HasData(new Service.Coupons.Api.Model.Coupon
            //{
            //    CouponId = 1,
            //    CouponCode = "10OFF",
            //    DiscountAmount = 10,
            //    MinAmount = 20,
            //});
            //modelBuilder.Entity<Service.Coupons.Api.Model.Coupon>().HasData(new Service.Coupons.Api.Model.Coupon
            //{
            //    CouponId = 2,
            //    CouponCode = "20OFF",
            //    DiscountAmount = 20,
            //    MinAmount = 40,
            //});

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}

        public DbSet<Service.Coupons.Api.Model.Coupon> Coupons { get; set; } = null!;

    }
}
