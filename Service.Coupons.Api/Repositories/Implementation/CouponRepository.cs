using Microsoft.EntityFrameworkCore;
using Service.Coupons.Api.Common.Exception;
using Service.Coupons.Api.Contracts.Interfaces;
using Service.Coupons.Api.Data;
using Service.Coupons.Api.Model;

namespace Service.Coupons.Api.Repositories.Implementation
{
    public class CouponRepository: ICouponRepository<Service.Coupons.Api.Model.Coupon>
    {

        private readonly AppDbContext _context;

        public CouponRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Service.Coupons.Api.Model.Coupon> CreateAsync(Service.Coupons.Api.Model.Coupon model)
        {
            try
            {
                if(model != null)
                {
                    model.StripeCouponId = "4b8a625f-66de-4c6f-b65e-36cde7270164";
                    var add = await _context.AddAsync(model);
                    
                    if(add.State == EntityState.Added)
                    {
                        await _context.SaveChangesAsync();
                        return add.Entity as Service.Coupons.Api.Model.Coupon;
                    }
                    else
                    {
                        throw new ArgumentException(nameof(add));
                    }
                }
                else
                {
                    throw new ArgumentNullException(nameof(model));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Service.Coupons.Api.Model.Coupon> DeleteAsync(int Id)
        {
            var findId = await _context.Coupons.FindAsync(Id);
            if (findId is null)
            {
                throw new IdNullException("Id is null");
            }
            else
            {
                var delete = _context.Remove(findId);
                await _context.SaveChangesAsync();
                return await Task.FromResult<Service.Coupons.Api.Model.Coupon>(delete.Entity);
            }
        }

        public async Task<IEnumerable<Service.Coupons.Api.Model.Coupon>> GetAllAsync()
        {
            try
            {
               var list = await _context.Coupons.AsNoTracking().ToListAsync();
                if(list.Count > 0)
                {
                    return list;
                }
                else
                {
                    throw new ModelNullException(nameof(list), "List is empty");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Service.Coupons.Api.Model.Coupon> GetByCodeAsync(string code)
        {
           var coderes = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponCode == code);
            if(coderes == null)
            {
                throw new ArgumentNullException(nameof(code));
            }
            return coderes;
        }

        public async Task<Service.Coupons.Api.Model.Coupon> GetByIdAsync(int id)
        {
            try
            {
                var find = await _context.Coupons.FindAsync(id);
                if(find != null)
                {
                    return find;
                }
                else
                {
                    throw new ArgumentNullException(nameof(id)); 
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<Service.Coupons.Api.Model.Coupon> UpdateAsync(Service.Coupons.Api.Model.Coupon model)
        {
            var findId = _context.Coupons.FirstOrDefault(c => c.CouponId == model.CouponId);
            if (findId != null)
            {
                _context.Entry(findId).CurrentValues.SetValues(model);
                _context.SaveChanges();
                return Task.FromResult<Service.Coupons.Api.Model.Coupon>(model);
            }
            else
            {
                throw new ArgumentNullException();
            }

        }
    }
}
