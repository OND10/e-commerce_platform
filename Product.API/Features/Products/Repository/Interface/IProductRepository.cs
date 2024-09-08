namespace Product.API.Features.Products.Repository.Interface
{
    public interface IProductRepository
    {
        Task<Entities.Product> CreateAsync(Entities.Product model);
        Task<IEnumerable<Entities.Product>> GetAllAsync();
        Task<Entities.Product> GetByIdAsync(int id);
        Task<Entities.Product> UpdateAsync(Entities.Product model);
        Task<bool> DeleteAsync(int Id); 
    }
}
