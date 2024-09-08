using Product.API.DataBase;
using Product.API.Features.Products.Repository.Interface;

namespace Product.API.Features.Products.Repository.Implementation
{
    public class UnitofWork : IUnitofWork
    {
        private readonly AppDbContext _context;
        public UnitofWork(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result;
        }
    }
}
