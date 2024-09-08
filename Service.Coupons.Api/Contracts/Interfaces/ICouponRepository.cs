namespace Service.Coupons.Api.Contracts.Interfaces
{
    public interface ICouponRepository<Coupon> where Coupon : class
    {

        Task<Coupon> CreateAsync(Coupon model);
        Task<IEnumerable<Coupon>> GetAllAsync();
        Task<Coupon> GetByIdAsync(int id);
        Task<Coupon> GetByCodeAsync(string code);
        Task<Coupon> UpdateAsync(Coupon model);
        Task<Coupon> DeleteAsync(int Id);

    }
}
