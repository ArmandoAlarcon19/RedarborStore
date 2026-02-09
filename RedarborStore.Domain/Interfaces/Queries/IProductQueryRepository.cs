using Domain.Entities;

namespace Domain.Interfaces.Queries;

public interface IProductQueryRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
}