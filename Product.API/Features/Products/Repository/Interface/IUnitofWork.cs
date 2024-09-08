namespace Product.API.Features.Products.Repository.Interface
{
    public interface IUnitofWork
    {
        Task<int> SaveChangesAsync();
    }
}
