using RedarborStore.Domain.Entities;

namespace RedarborStore.Domain.Interfaces.Queries;

public interface IProductQueryRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
}