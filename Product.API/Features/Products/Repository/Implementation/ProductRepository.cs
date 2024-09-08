using Microsoft.EntityFrameworkCore;
using Product.API.Common.Exceptions;
using Product.API.DataBase;
using Product.API.Features.Products.Repository.Interface;

namespace Product.API.Features.Products.Repository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Entities.Product> CreateAsync(Entities.Product model)
        {
            try
            {
                var add = await _context.AddAsync(model);

                return await Task<Entities.Product>.FromResult<Entities.Product>(add.Entity);
            }
            catch
            {
                throw new ModelNullException($"{model}", "Model is null");
            }
        }


        public async Task<IEnumerable<Entities.Product>> GetAllAsync()
        {
            var list = await _context.Products.AsNoTrackingWithIdentityResolution().ToListAsync();
            try
            {
                list = await _context.Products.ToListAsync();
                if (list.Count < 0)
                {
                    throw new ModelNullException($"{list}", "List is null");
                }
                else
                {
                    return await Task<IEnumerable<Entities.Product>>.FromResult<IEnumerable<Entities.Product>>(list);
                }
            }
            catch
            {
                throw new ModelNullException($"{list}", "List is null");
            }
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            var findId = await _context.Products.FindAsync(Id);
            if (findId is null)
            {
                throw new IdNullException("Id is null");
            }
            else
            {
                var delete = _context.Remove(findId);
                return await Task.FromResult<bool>(true);
            }
        }


        public async Task<Entities.Product> GetByIdAsync(int id)
        {
            try
            {
                var find = await _context.Products.FindAsync(id);
                if (find != null)
                {
                    return find;
                }
                else
                {
                    throw new IdNullException(nameof(id));
                }
            }
            catch
            {
                throw new IdNullException($"Id is null");
            }
        }

        public async Task<Entities.Product> UpdateAsync(Entities.Product model)
        {
            var findId = _context.Products.FirstOrDefault(c => c.Id == model.Id);
            if (findId != null)
            {
                _context.Entry(findId).CurrentValues.SetValues(model);
                return await Task.FromResult<Entities.Product>(model);
            }
            else
            {
                throw new IdNullException("Id is null");
            }
        }
    }
}
